const float _Gravity = 9.81;

// struct ErosionTile
// {
//     float b; // Terrain height
//     float d; // Water height
//     float s; // Suspended sediment amount
//     float4 f; // Water outflow flux (L, R, T, B)
//     float2 v; // Velocity vector
// };
struct ErosionTile
{
    float tHeight; // Terrain height
    float wHeight; // Water height
    float sediment;
    float hardness;
    float2 velocity;
    float4 waterFlux; // r - left, g - right, b - top, a - bottom
    float4 terrainFlux; // r - left, g - right, b - top, a - bottom
};

#define ErosionTileDefault(newTile)\
ErosionTile newTile;\
newTile.tHeight = 0.0;\
newTile.wHeight = 0.1;\
newTile.sediment = 0.1;\
newTile.hardness = 1.0;\
newTile.velocity = float2(0.0, 0.0);\
newTile.waterFlux = float4(0.0, 0.0, 0.0, 0.0);\
newTile.terrainFlux = float4(0.0, 0.0, 0.0, 0.0);

// Flattened tilemaps of floats representing water heights for hydraulic erosion
RWStructuredBuffer<ErosionTile> _TileMapHydraulic0;
RWStructuredBuffer<ErosionTile> _TileMapHydraulic1;

float _SedimentCapacity, _MaxErosionDepth, _SuspensionRate, _DepositionRate, _SedimentSofteningRate,
      _ThermalErosionTimeScale, _ThermalErosionRate, _TalusAngleTangentCoeff, _TalusAngleTangentBias,
      _DeltaTime, _PipeArea, _PipeLength, _Evaporation, _RainRate;
float2 _CellScale;
// float _CellScale, _TanThresholdAngle, _ErosionAmplitude, _DeltaTime, _SedimentCapacity, _MaxErosionDepth;

/**
 * Return the value of the tile at xy in the currently buffered array
 */
ErosionTile GetHydraulicTileAt(uint2 xy)
{
    if (xy.x < _ScaleX && xy.y < _ScaleY)
    {
        if (_BufferFlag)
            return _TileMapHydraulic1[xy.x + xy.y * _ScaleX];
        else
            return _TileMapHydraulic0[xy.x + xy.y * _ScaleX];
    }
    else
    {
        ErosionTileDefault(tile);
        return tile;
    }
}
/**
 * Set the value of the tile at xy in the currently buffered array
 */
void SetHydraulicTileAt(uint2 xy, ErosionTile value)
{
    if (xy.x < _ScaleX && xy.y < _ScaleY)
    {
        if (_BufferFlag)
            _TileMapHydraulic0[xy.x + xy.y * _ScaleX] = value;
        else
            _TileMapHydraulic1[xy.x + xy.y * _ScaleX] = value;
    }
}
/**
 * Set the value of the tile at xy in the currently buffered array
 */
void SetBothHydraulicTileAt(uint2 xy, ErosionTile value)
{
    if (xy.x < _ScaleX && xy.y < _ScaleY)
    {
        _TileMapHydraulic0[xy.x + xy.y * _ScaleX] = value;
        _TileMapHydraulic1[xy.x + xy.y * _ScaleX] = value;
    }
}

// float LMax(float x)
// {
//     if (x <= 0)
//         return 0;
//     else if (x >= _MaxErosionDepth)
//         return 1;
//     else
//         return 1 - (_MaxErosionDepth - x) / _MaxErosionDepth;
// }

// https://github.com/bshishov/UnityTerrainErosionGPU/blob/master/Assets/Shaders/Erosion.compute
[numthreads(8, 8, 1)]
void ThermalErosionPass1(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile  = GetHydraulicTileAt(id.xy               );
    ErosionTile tileL = GetHydraulicTileAt(id.xy + int2(-1,  0));
    ErosionTile tileR = GetHydraulicTileAt(id.xy + int2( 1,  0));
    ErosionTile tileT = GetHydraulicTileAt(id.xy + int2( 0,  1));
    ErosionTile tileB = GetHydraulicTileAt(id.xy + int2( 0, -1));

    float4 neighbourHeights = float4(tileL.tHeight, tileR.tHeight, tileT.tHeight, tileB.tHeight);
    float4 heightDifference = max(0, tile.tHeight - neighbourHeights);
    float maxHeightDifference = max(max(heightDifference.x, heightDifference.y), max(heightDifference.z, heightDifference.w));

    float volumeToBeMoved = _CellScale.x * _CellScale.y * maxHeightDifference * 0.5 * _ThermalErosionRate * tile.hardness;

    float4 tanAngle = float4(heightDifference.xy / _CellScale.x, heightDifference.zw / _CellScale.y);

    float threshold = tile.hardness * _TalusAngleTangentCoeff + _TalusAngleTangentBias;

    float4 k = 0;
    if (tanAngle.x > threshold)
        k.x = heightDifference.x;
    if (tanAngle.y > threshold)
        k.y = heightDifference.y;
    if (tanAngle.z > threshold)
        k.z = heightDifference.z;
    if (tanAngle.w > threshold)
        k.w = heightDifference.w;
    
    float sumProportions = k.x + k.y + k.z + k.w;
    float4 outputFlux = 0;
    if (sumProportions > 0)
        outputFlux = volumeToBeMoved * k / sumProportions;
    
    if (id.x == 0)
        outputFlux.r = 0;
    if (id.y == 0)
        outputFlux.a = 0;
    if (id.x == _ScaleY - 1)
        outputFlux.g = 0;
    if (id.y == _ScaleY - 1)
        outputFlux.b = 0;

    tile.terrainFlux = outputFlux;
    SetHydraulicTileAt(id.xy, tile);
}

[numthreads(8, 8, 1)]
void ThermalErosionPass2(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile  = GetHydraulicTileAt(id.xy               );
    ErosionTile tileL = GetHydraulicTileAt(id.xy + int2(-1,  0));
    ErosionTile tileR = GetHydraulicTileAt(id.xy + int2( 1,  0));
    ErosionTile tileT = GetHydraulicTileAt(id.xy + int2( 0,  1));
    ErosionTile tileB = GetHydraulicTileAt(id.xy + int2( 0, -1));

    float4 inputFlux = float4(tileL.terrainFlux.g, tileR.terrainFlux.r, tileB.terrainFlux.a, tileT.terrainFlux.b);

    float volumeDelta = inputFlux.r + inputFlux.g + inputFlux.b + inputFlux.a - tile.terrainFlux.r - tile.terrainFlux.g - tile.terrainFlux.b - tile.terrainFlux.a;

    tile.tHeight += min(1, _DeltaTime * _ThermalErosionTimeScale) * volumeDelta;

    SetHydraulicTileAt(id.xy, tile);
}

// Rain simulation
[numthreads(8, 8, 1)]
void HydraulicErosionPass1(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile = GetHydraulicTileAt(id.xy);
    tile.wHeight += _DeltaTime * _RainRate;
    SetHydraulicTileAt(id.xy, tile);
}

// Flux field computation
[numthreads(8, 8, 1)]
void HydraulicErosionPass2(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile  = GetHydraulicTileAt(id.xy               );
    ErosionTile tileL = GetHydraulicTileAt(id.xy + int2(-1,  0));
    ErosionTile tileR = GetHydraulicTileAt(id.xy + int2( 1,  0));
    ErosionTile tileT = GetHydraulicTileAt(id.xy + int2( 0,  1));
    ErosionTile tileB = GetHydraulicTileAt(id.xy + int2( 0, -1));

    float4 heightDifference = (tile.tHeight + tile.wHeight) - float4(
        tileL.tHeight + tileL.wHeight,
        tileR.tHeight + tileR.wHeight,
        tileT.tHeight + tileT.wHeight,
        tileB.tHeight + tileB.wHeight
    );

    tile.waterFlux = max(0, tile.waterFlux + _DeltaTime * _Gravity * _PipeArea * heightDifference / _PipeLength);
    tile.waterFlux *= min(1, tile.wHeight * _CellScale.x * _CellScale.y
        / ((tile.waterFlux.r + tile.waterFlux.g + tile.waterFlux.b + tile.waterFlux.a) * _DeltaTime));
    
    if (id.x == 0)
        tile.waterFlux.r = 0;
    if (id.y == 0)
        tile.waterFlux.a = 0;
    if (id.x == _ScaleY - 1)
        tile.waterFlux.g = 0;
    if (id.y == _ScaleY - 1)
        tile.waterFlux.b = 0;
    
    SetHydraulicTileAt(id.xy, tile);
}

// Application of fluxes to actual tiles and changing water height
[numthreads(8, 8, 1)]
void HydraulicErosionPass3(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile  = GetHydraulicTileAt(id.xy               );
    ErosionTile tileL = GetHydraulicTileAt(id.xy + int2(-1,  0));
    ErosionTile tileR = GetHydraulicTileAt(id.xy + int2( 1,  0));
    ErosionTile tileT = GetHydraulicTileAt(id.xy + int2( 0,  1));
    ErosionTile tileB = GetHydraulicTileAt(id.xy + int2( 0, -1));

    float4 inputFlux = float4(tileL.waterFlux.g, tileR.waterFlux.r, tileB.waterFlux.a, tileT.waterFlux.b);
    float waterHeightBefore = tile.wHeight;

    float volumeDelta = inputFlux.r + inputFlux.g + inputFlux.b + inputFlux.a
        - tile.waterFlux.r - tile.waterFlux.g - tile.waterFlux.b - tile.waterFlux.a;

    tile.wHeight += _DeltaTime * volumeDelta / (_CellScale.x * _CellScale.y);

    tile.velocity = float2(
        0.5 * (inputFlux.r - tile.waterFlux.r + inputFlux.g - tile.waterFlux.g),
        0.5 * (inputFlux.a - tile.waterFlux.a + inputFlux.b - tile.waterFlux.b)
    );

    SetHydraulicTileAt(id.xy, tile);
}


[numthreads(8, 8, 1)]
void HydraulicErosionPass4(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile  = GetHydraulicTileAt(id.xy               );
    ErosionTile tileL = GetHydraulicTileAt(id.xy + int2(-1,  0));
    ErosionTile tileR = GetHydraulicTileAt(id.xy + int2( 1,  0));
    ErosionTile tileT = GetHydraulicTileAt(id.xy + int2( 0,  1));
    ErosionTile tileB = GetHydraulicTileAt(id.xy + int2( 0, -1));

    float3 dhdx = float3(2 * _CellScale.x, tileR.tHeight - tileL.tHeight, 0);
    float3 dhdy = float3(0, tileT.tHeight - tileB.tHeight, 2 * _CellScale.y);
    float3 normal = cross(dhdx, dhdy);

    float sinTiltAngle = abs(normal.y) / length(normal);

    float lmax = saturate(1 - max(0, _MaxErosionDepth - tile.wHeight)) / _MaxErosionDepth;
    float sedimentTransportCapacity = _SedimentCapacity * length(tile.velocity) * min(sinTiltAngle, 0.05) * lmax;

    if (tile.sediment < sedimentTransportCapacity)
    {
        float mod = _DeltaTime * _SuspensionRate * tile.hardness * (sedimentTransportCapacity - tile.sediment);
        tile.tHeight -= mod;
        tile.sediment += mod;
        tile.wHeight += mod;
    }
    else
    {
        float mod = _DeltaTime * _DepositionRate * (tile.sediment - sedimentTransportCapacity);
        tile.tHeight += mod;
        tile.sediment -= mod;
        tile.wHeight -= mod;
    }

    tile.wHeight *= 1 - _Evaporation * _DeltaTime;

    tile.hardness = tile.hardness - _DeltaTime * _SedimentSofteningRate * _SuspensionRate * (tile.sediment - sedimentTransportCapacity);
    tile.hardness = clamp(tile.hardness, 0, 1);

    SetHydraulicTileAt(id.xy, tile);
}

// Application of fluxes to actual tiles and changing water height
[numthreads(8, 8, 1)]
void HydraulicErosionPass5(uint3 id: SV_DispatchThreadID)
{
    ErosionTile tile = GetHydraulicTileAt(id.xy);

    float2 uv = id.xy - tile.velocity * _DeltaTime;

    float2 uva = floor(uv);
    float2 uvb = ceil(uv);

    uint2 id00 = (uint2) uva;
    uint2 id10 = uint2(uvb.x, uva.y);
    uint2 id01 = uint2(uva.x, uvb.y);
    uint2 id11 = (uint2)uvb;

    float2 d = uv - uva;

    tile.sediment = GetHydraulicTileAt(id00).tHeight * (1 - d.x) * (1 - d.y)
                  + GetHydraulicTileAt(id10).tHeight * d.x * (1 - d.y)
                  + GetHydraulicTileAt(id01).tHeight * (1 - d.x) * d.y
                  + GetHydraulicTileAt(id11).tHeight * d.x * d.y;
    
    SetHydraulicTileAt(id.xy, tile);
}

[numthreads(8, 8, 1)]
void FluvialErosion(uint3 id: SV_DispatchThreadID)
{
    
}

[numthreads(8, 8, 1)]
void InitErosionTileMap(uint3 id: SV_DispatchThreadID)
{
    ErosionTileDefault(tile);
    tile.tHeight = GetContTileAt(id.xy);
    SetBothHydraulicTileAt(id.xy, tile);
}

[numthreads(8, 8, 1)]
void UpdateErosionTileMap(uint3 id: SV_DispatchThreadID)
{
    SetContTileAt(id.xy, GetHydraulicTileAt(id.xy).tHeight);
}
