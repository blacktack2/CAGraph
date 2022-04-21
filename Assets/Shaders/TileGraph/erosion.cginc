struct ErosionTile
{
    float landH;  // Terrain Height
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

[numthreads(8, 8, 1)]
void HydraulicErosionStreamPowerLaw(uint3 id: SV_DispatchThreadID)
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

// Based on https://www.shadertoy.com/view/XsKGWG#
[numthreads(8, 8, 1)]
void HydraulicErosionPoor(uint3 id: SV_DispatchThreadID)
{
    float2 uv = id.xy + _Offset.xy;
 
    ErosionTile tiles[9];
    tiles[0] = GetWrapErosionTileAt(id.xy);
    tiles[1] = GetWrapErosionTileAt(id.xy + int2( 1,  0));
    tiles[2] = GetWrapErosionTileAt(id.xy + int2(-1,  0));
    tiles[3] = GetWrapErosionTileAt(id.xy + int2( 0,  1));
    tiles[4] = GetWrapErosionTileAt(id.xy + int2( 0, -1));
    tiles[5] = GetWrapErosionTileAt(id.xy + int2( 1,  1));
    tiles[6] = GetWrapErosionTileAt(id.xy + int2(-1,  1));
    tiles[7] = GetWrapErosionTileAt(id.xy + int2( 1, -1));
    tiles[8] = GetWrapErosionTileAt(id.xy + int2(-1, -1));

    float landHC  = tiles[0].landH;  // Land height of current cell
    float waterVC = tiles[0].waterV; // Water volume of current cell
    float waterHC = landHC + waterVC;

    float landH  = landHC;
    float waterV = waterVC;    
    for (int i = 1; i < 9; i++)
    {
        float landHi  = tiles[i].landH;
        float waterVi = tiles[i].waterV;
        float waterHi = landHi + waterVi;
        float waterSlope = waterHi - waterHC;
        float landSlope  = landHi  - landHC ;

        if (i > 4)
        {
            // Normalize corner weights
            waterSlope *= 0.70710678118;
            landSlope  *= 0.70710678118;
        }

        if (waterVC > 0.0 && waterSlope < 0.0)
        {
            waterV += waterSlope * (1.0 / 9.0);
            landH += 0.003 * waterSlope; // Deposition
        }
        if (waterVi > 0.0 && waterSlope > 0.0)
        {
            waterV += waterSlope * (1.0 / 12.0);
            landH -= 0.003 * waterSlope; // Erosion
        }

        landH += 0.001 * waterSlope;

        if (landSlope < -0.002 - 0.004 * hash12(uv))
            landH += landSlope * (1.0 / 9.0);
    }
    landH += 0.001 * waterV;
    waterV -= 1.0 / 65535.0;
    
    if (hash12((uv + _Iteration * 0.001) % 100.) > 0.5)
        waterV += 4.0 / 65535.0;

    waterV *= 0.98;
    
    ErosionTile tile;
    tile.landH = landH;
    tile.waterV = waterV;
    SetErosionTileAt(id.xy, tile);
}