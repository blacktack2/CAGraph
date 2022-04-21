using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Erosion : SubLibrary
        {
            public struct ErosionTile
            {
                public float landH;
                public float waterV;
            }

            private Hydraulic _Hydraulic;
            public Hydraulic hydraulic {get {return _Hydraulic;}}

            public Erosion(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
                _Hydraulic = new Hydraulic(functionLibrary);
            }

            public class Hydraulic : SubLibrary
            {
                public enum Algorithm
                {
                    PoorErosion,
                    StreamPowerLaw,
                }
                
                public Hydraulic(FunctionLibrary functionLibrary) : base(functionLibrary)
                {
                }

                public void StreamPowerLaw(Types.TileMapCont tileMap, int iterations = 1, bool useGPU = true)
                {
                    if (useGPU)
                        StreamPowerLawGPU(tileMap, iterations);
                    else
                        StreamPowerLawCPU(tileMap, iterations);
                }
                private void StreamPowerLawCPU(Types.TileMapCont tileMap, int iterations)
                {

                }
                private void StreamPowerLawGPU(Types.TileMapCont tileMap, int iterations)
                {
                    const int kernelIndex = (int) FunctionKernels.HydraulicErosionStreamPowerLaw;

                    float[] cells = tileMap.GetCells();
                    ErosionTile[] tiles = new ErosionTile[cells.Length];
                    for (int i = 0; i < tiles.Length; i++)
                        tiles[i] = new ErosionTile() {landH = cells[i], waterV = 0f};
                    
                    _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                    _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                    
                    _FunctionLibrary._TileMapErosion0Buffer.SetData(tiles);
                    _FunctionLibrary._TileMapErosion1Buffer.SetData(tiles);

                    int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                    int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                    bool bufferFlag = false;
                    for (int i = 0; i < iterations; i++)
                    {
                        bufferFlag = !bufferFlag;
                        _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                        _FunctionLibrary._ComputeShader.SetInt(_IterationID, i);

                        _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);
                    }

                    if (bufferFlag)
                        _FunctionLibrary._TileMapErosion0Buffer.GetData(tiles);
                    else
                        _FunctionLibrary._TileMapErosion1Buffer.GetData(tiles);
                    
                    for (int i = 0; i < cells.Length; i++)
                        cells[i] = tiles[i].landH;
                    tileMap.SetCells(cells); 
                }

                public void Poor(Types.TileMapCont tileMap, int iterations = 1, bool useGPU = true)
                {
                    if (useGPU)
                        PoorGPU(tileMap, iterations);
                    else
                        PoorCPU(tileMap, iterations);
                }
                private void PoorCPU(Types.TileMapCont tileMap, int iterations)
                {

                }
                private void PoorGPU(Types.TileMapCont tileMap, int iterations)
                {
                    const int kernelIndex = (int) FunctionKernels.HydraulicErosionPoor;

                    float[] cells = tileMap.GetCells();
                    ErosionTile[] tiles = new ErosionTile[cells.Length];
                    for (int i = 0; i < tiles.Length; i++)
                        tiles[i] = new ErosionTile() {landH = cells[i], waterV = 0f};
                    
                    _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                    _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                    
                    _FunctionLibrary._TileMapErosion0Buffer.SetData(tiles);
                    _FunctionLibrary._TileMapErosion1Buffer.SetData(tiles);

                    int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                    int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                    bool bufferFlag = false;
                    for (int i = 0; i < iterations; i++)
                    {
                        bufferFlag = !bufferFlag;
                        _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                        _FunctionLibrary._ComputeShader.SetInt(_IterationID, i);

                        _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);
                    }

                    if (bufferFlag)
                        _FunctionLibrary._TileMapErosion0Buffer.GetData(tiles);
                    else
                        _FunctionLibrary._TileMapErosion1Buffer.GetData(tiles);
                    
                    for (int i = 0; i < cells.Length; i++)
                        cells[i] = tiles[i].landH;
                    tileMap.SetCells(cells); 
                }
            }
        }
    }
}
