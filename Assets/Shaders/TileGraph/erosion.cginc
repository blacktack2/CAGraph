float _TerrainHardness, _SedimentHardness, _DepositionRate,
    _RainRate, _RainAmount,
    _MaxSlope, _ThermalRate;

struct ErosionTile
{
    float landH;  // Terrain Height
    float sedH;   // Sediment Height
    float waterV; // Water Volume
};
ErosionTile GetDefaultErosionTile()
{
    ErosionTile tile;
    tile.landH = 0.0;
    tile.waterV = 0.0;
    return tile;
}
DefineTileMap(Erosion, ErosionTile, GetDefaultErosionTile()) // GetErosionTileAt() GetClampErosionTileAt SetErosionTileAt()

float hash12(float2 p)
{
    float3 p3 = frac(float3(p.xyx) * float3(443.8975, 397.2973, 491.1871));
    p3 += dot(p3, p3.yzx + 19.19);
    return frac((p3.x + p3.y) * p3.z);
}

#define slope(p, q) (GetWrapErosionTileAt(q).landH - GetWrapErosionTileAt(p).landH) / distance(p, q)

float2 rec(float2 t)
{
    float2 d = N;
    if (slope(t + NE, t) > slope(t + d, t))
        d = NE;
    if (slope(t + E , t) > slope(t + d, t))
        d = E;
    if (slope(t + SE, t) > slope(t + d, t))
        d = SE;
    if (slope(t + S , t) > slope(t + d, t))
        d = S;
    if (slope(t + SW, t) > slope(t + d, t))
        d = SW;
    if (slope(t + W , t) > slope(t + d, t))
        d = W;
    if (slope(t + NW, t) > slope(t + d, t))
        d = NW;
    return d;
}

#define softErode(amount)\
sedH -= _SedimentHardness * amount;\
if (sedH < 0.0)\
{\
    landH += sedH * _TerrainHardness / _SedimentHardness;\
    sedH = 0.0;\
}

// Hydraulic Erosion, based on: https://www.shadertoy.com/view/XsKGWG#
[numthreads(8, 8, 1)]
void HydraulicErosion(uint3 id: SV_DispatchThreadID)
{
    float2 uv = id.xy + _Offset.xy;
 
    ErosionTile tiles[9];
    tiles[0] = GetWrapErosionTileAt(id.xy);
    // Cardinal neighbours
    tiles[1] = GetClampErosionTileAt(id.xy + N);
    tiles[2] = GetClampErosionTileAt(id.xy + E);
    tiles[3] = GetClampErosionTileAt(id.xy + S);
    tiles[4] = GetClampErosionTileAt(id.xy + W);
    // Corner neighbours
    tiles[5] = GetClampErosionTileAt(id.xy + NE);
    tiles[6] = GetClampErosionTileAt(id.xy + SE);
    tiles[7] = GetClampErosionTileAt(id.xy + SW);
    tiles[8] = GetClampErosionTileAt(id.xy + NW);

    float landHC  = tiles[0].landH;  // Land height of current cell
    float sedHC   = tiles[0].sedH;   // Sediment height of current cell
    float waterVC = tiles[0].waterV; // Water volume of current cell
    float waterHC = landHC + sedHC + waterVC;

    // Return values
    float landH  = landHC;
    float sedH   = sedHC;
    float waterV = waterVC;
    for (int i = 1; i < 9; i++)
    {
        float landHi  = tiles[i].landH;
        float sedHi   = tiles[i].sedH;
        float waterVi = tiles[i].waterV;
        float waterHi = landHi + sedHi + waterVi;
        float waterSlope = waterHi - waterHC;
        float landSlope  = landHi + sedHi - landHC - sedHC;

        if (i > 4)
        {
            // Normalize corner weights (1 / sqrt(2))
            waterSlope *= 0.70710678118;
            landSlope  *= 0.70710678118;
        }

        if (waterVC > 0.0 && waterSlope < 0.0)
        {
            // Reduce water (outflow) and erode terrain
            waterV += waterSlope * (1.0 / 9.0);
            softErode(0.003 * waterSlope)
            // landH += _TerrainHardness * 0.003 * waterSlope;
        }
        if (waterVi > 0.0 && waterSlope > 0.0)
        {
            // Increase water (inflow) and erode terrain
            waterV += waterSlope * (1.0 / 12.0);
            softErode(-0.003 * waterSlope);
            // landH -= _TerrainHardness * 0.003 * waterSlope;
        }

        // Erode/deposit based on slope
        if (waterSlope > 0)
        {
            sedH += _DepositionRate * 0.001 * waterSlope;
        }
        else
        {
            softErode(-0.001 * waterSlope)
            // landH += _TerrainHardness * 0.001 * waterSlope;
        }

        // Erode steep slopes
        if (landSlope < -0.002 - 0.004 * hash12(uv))
        {
            softErode(-landSlope * (1.0 / 9.0))
            // landH += _TerrainHardness * landSlope * (1.0 / 9.0);
        }
    }

    sedH += _DepositionRate * 0.001 * waterV; // Deposition
    waterV -= 1.0 / 65535.0; // Evaporation
    
    // Rain
    if (hash12((uv + _Iteration * 0.001) % 100.) > _RainRate)
        waterV += _RainAmount * 4.0 / 65535.0;

    waterV *= 0.98;
    
    ErosionTile tile;
    tile.landH = landH;
    tile.sedH = sedH;
    tile.waterV = waterV;
    SetErosionTileAt(id.xy, tile);
}

// Stream Power Law, based on: https://www.shadertoy.com/view/XsVBRm
[numthreads(8, 8, 1)]
void FluvialErosion(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile = GetErosionTileAt(id.xy);
    float landH  = tile.landH;
    float waterV = 1.0;

    if (distance(rec(id.xy + N ), -N ) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + N ).waterV;
    if (distance(rec(id.xy + NE), -NE) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + NE).waterV;
    if (distance(rec(id.xy + E ), -E ) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + E ).waterV;
    if (distance(rec(id.xy + SE), -SE) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + SE).waterV;
    if (distance(rec(id.xy + S ), -S ) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + S ).waterV;
    if (distance(rec(id.xy + SW), -SW) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + SW).waterV;
    if (distance(rec(id.xy + W ), -W ) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + W ).waterV;
    if (distance(rec(id.xy + NW), -NW) < 0.001)
        waterV += GetWrapErosionTileAt(id.xy + NW).waterV;

    ErosionTile receiver = GetWrapErosionTileAt(id.xy + rec(id.xy));
    float tileSlope = (landH - receiver.landH) / length(rec(id.xy));
    landH = max(landH - 0.05 * PowN(waterV, 0.8) * PowN(tileSlope, 2.0), receiver.landH); 
    
    tile.landH = landH;
    tile.waterV = waterV;
    SetErosionTileAt(id.xy, tile);
}

// Thermal Erosion, based on: https://www.shadertoy.com/view/XtKSWh
[numthreads(8, 8, 1)]
void ThermalErosion(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tiles[9];
    tiles[0] = GetErosionTileAt(id.xy);
    // Cardinal neighbours
    tiles[1] = GetClampErosionTileAt(id.xy + N);
    tiles[2] = GetClampErosionTileAt(id.xy + E);
    tiles[3] = GetClampErosionTileAt(id.xy + S);
    tiles[4] = GetClampErosionTileAt(id.xy + W);
    // Corner neighbours
    tiles[5] = GetClampErosionTileAt(id.xy + NE);
    tiles[6] = GetClampErosionTileAt(id.xy + SE);
    tiles[7] = GetClampErosionTileAt(id.xy + SW);
    tiles[8] = GetClampErosionTileAt(id.xy + NW);

    float landH = tiles[0].landH;

    float mDif = 0.0;
    for (int i = 1; i < 9; i++)
    {
        float dist = (i > 4 ? 1.41421356237 : 1.0); // Offset by sqrt(2) if a corner neighbour
        float maxDif = _MaxSlope * dist / 400.0;
        float landHi = tiles[i].landH;
        if (landH > landHi)
        {
            // Lower current tile height closer to tile i
            float diff = landH - landHi;
            if (diff > maxDif)
                mDif -= _ThermalRate * (diff - maxDif) / dist;
        }
        else
        {
            // Raise current tile height closer to tile i
            float diff = landHi - landH;
            if (diff > maxDif)
                mDif += _ThermalRate * (diff - maxDif) / dist;
        }
    }

    tiles[0].landH += mDif;
    SetErosionTileAt(id.xy, tiles[0]);
}