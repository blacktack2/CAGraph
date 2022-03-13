/**
 * Return the number of neighbours in the Moore neighbourhood of the cell at xy
 */
uint CountNeighboursAt(uint2 xy)
{
    return GetBoolTileAt(xy + int2(-1, -1)) +
           GetBoolTileAt(xy + int2(-1,  0)) +
           GetBoolTileAt(xy + int2(-1,  1)) +
           GetBoolTileAt(xy + int2( 0, -1)) +
           GetBoolTileAt(xy + int2( 0,  1)) +
           GetBoolTileAt(xy + int2( 1, -1)) +
           GetBoolTileAt(xy + int2( 1,  0)) +
           GetBoolTileAt(xy + int2( 1,  1));
}

[numthreads(8, 8, 1)]
void IterateLifeCells(uint3 id: SV_DispatchThreadID)
{
    SetBoolTileAt(id.xy, _LifeRules[GetBoolTileAt(id.xy) * 9 + CountNeighboursAt(id.xy)]);
}
