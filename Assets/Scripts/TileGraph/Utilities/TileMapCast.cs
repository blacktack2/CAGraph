using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class TileMapCast : SubLibrary
        {
            public TileMapCast(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            /// <summary> Cast a <paramref name="TileMapBool" /> to a
            /// <paramref name="TileMapCont" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapBool" /> to cast.
            /// </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public Types.TileMapCont CastBoolToCont(Types.TileMapBool tileMap, bool useGPU = true)
            {
                int[] cells = tileMap.GetCells();
                float[] newCells = new float[cells.Length];
                for (int i = 0; i < cells.Length; i++)
                    newCells[i] = (float) cells[i];
                Types.TileMapCont newTileMap = new Types.TileMapCont(tileMap.width, tileMap.height);
                newTileMap.SetCells(newCells);
                return newTileMap;
            }
            /// <summary> Cast a <paramref name="TileMapBool" /> to a
            /// <paramref name="TileMapUint" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapBool" /> to cast.
            /// </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public Types.TileMapUint CastBoolToUint(Types.TileMapBool tileMap, bool useGPU = true)
            {
                int[] cells = tileMap.GetCells();
                uint[] newCells = new uint[cells.Length];
                for (int i = 0; i < cells.Length; i++)
                    newCells[i] = (uint) cells[i];
                Types.TileMapUint newTileMap = new Types.TileMapUint(tileMap.width, tileMap.height);
                newTileMap.SetCells(newCells);
                return newTileMap;
            }
            /// <summary> Cast a <paramref name="TileMapCont" /> to a
            /// <paramref name="TileMapBool" />. Values from
            /// 0-<paramref name="threshold" /> will become 0, and values from
            /// <paramref name="TileMapCont" />-1 will become 1 </summary>
            /// <param name="tileMap"> <paramref name="TileMapCont" /> to cast.
            /// </param>
            /// <param name="threshold"> Threshold value, below which all values
            /// are cast to 0. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public Types.TileMapBool CastContToBool(Types.TileMapCont tileMap, float threshold = 0.5f, bool useGPU = true)
            {
                float[] cells = tileMap.GetCells();
                int[] newCells = new int[cells.Length];
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i] < threshold)
                        newCells[i] = 0;
                    else
                        newCells[i] = 1;
                }
                Types.TileMapBool newTileMap = new Types.TileMapBool(tileMap.width, tileMap.height);
                newTileMap.SetCells(newCells);
                return newTileMap;
            }
            /// <summary> Cast a <paramref name="TileMapCont" /> to a
            /// <paramref name="TileMapUint" />. Values will be normalized from 0-1
            /// to 0-<paramref name="max" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapCont" /> to cast.
            /// </param>
            /// <param name="max"> Maximum value to normalize to. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public Types.TileMapUint CastContToUint(Types.TileMapCont tileMap, uint max = 1, bool useGPU = true)
            {
                float[] cells = tileMap.GetCells();
                uint[] newCells = new uint[cells.Length];
                for (int i = 0; i < cells.Length; i++)
                    newCells[i] = (uint) Mathf.Round(cells[i] * max);
                Types.TileMapUint newTileMap = new Types.TileMapUint(tileMap.width, tileMap.height);
                newTileMap.SetCells(newCells);
                return newTileMap;
            }
            /// <summary> Cast a <paramref name="TileMapUint" /> to a
            /// <paramref name="TileMapBool" />. Values will be converted to 0 and
            /// 1 based on <paramref name="threshold" />. </summary>
            /// <param name="tileMap"> <paramref name="TileMapUint" /> to cast.
            /// </param>
            /// <param name="threshold"> Lowest value in
            /// <paramref name="tileMap" /> to be considered a 1. All other values
            /// will become 0. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public Types.TileMapBool CastUintToBool(Types.TileMapUint tileMap, uint threshold = 1, bool useGPU = true)
            {
                uint[] cells = tileMap.GetCells();
                int[] newCells = new int[cells.Length];
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i] < threshold)
                        newCells[i] = 0;
                    else
                        newCells[i] = 1;
                }
                Types.TileMapBool newTileMap = new Types.TileMapBool(tileMap.width, tileMap.height);
                newTileMap.SetCells(newCells);
                return newTileMap;
            }
            /// <summary> Cast a <paramref name="TileMapUint" /> to a
            /// <paramref name="TileMapCont" />. Values will be normalized from
            /// 0-<paramref name="max" /> to 0-1. </summary>
            /// <param name="tileMap"> <paramref name="TileMapUint" /> to cast.
            /// </param>
            /// <param name="max"> Maximum uint value to expect from
            /// <paramref name="tileMap" />. Values greater than
            /// <paramref name="max" /> will be clamped at 1. </param>
            /// <param name="useGPU"> If <c>true</c>, run using compute shaders
            /// else run using CPU. (Not yet implemented, does nothing) </param>
            public Types.TileMapCont CastUintToCont(Types.TileMapUint tileMap, uint max = 1, bool useGPU = true)
            {
                uint[] cells = tileMap.GetCells();
                float[] newCells = new float[cells.Length];
                for (int i = 0; i < cells.Length; i++)
                    newCells[i] = Mathf.Clamp01((float) cells[i] / (float) max);
                Types.TileMapCont newTileMap = new Types.TileMapCont(tileMap.width, tileMap.height);
                newTileMap.SetCells(newCells);
                return newTileMap;
            }
        }
    }
}
