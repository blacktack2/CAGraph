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

// Based on https://www.shadertoy.com/view/XsKGWG#
[numthreads(8, 8, 1)]
void HydraulicErosion(uint3 id: SV_DispatchThreadID)
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