using Unity.Mathematics;
using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Erosion : SubLibrary
        {
            public Erosion(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            public void ThermalErosion(Types.TileMapCont tileMap, float amplitude = 1f, float cellScale = 1f, float tanThresholdAngle = 0f, int iterations = 1, bool useGPU = true)
            {
                if (useGPU)
                    ThermalErosionGPU(tileMap, amplitude, cellScale, tanThresholdAngle, iterations);
                else
                    ThermalErosionCPU(tileMap, amplitude, cellScale, tanThresholdAngle, iterations);
            }

            private void ThermalErosionCPU(Types.TileMapCont tileMap, float amplitude, float cellScale, float tanThresholdAngle, int iterations)
            {
                
            }

            private void ThermalErosionGPU(Types.TileMapCont tileMap, float amplitude, float cellScale, float tanThresholdAngle, int iterations)
            {
                int kernelIndex = (int) FunctionKernels.ThermalErosionPass1;

                float[] cells = tileMap.GetCells();

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                // _FunctionLibrary._ComputeShader.SetFloat(_ErosionAmplitudeID, amplitude);
                _FunctionLibrary._ComputeShader.SetFloat(_CellScaleID, cellScale);
                // _FunctionLibrary._ComputeShader.SetFloat(_ErosionAmplitudeID, tanThresholdAngle);

                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                for (int i = 0; i < iterations; i++)
                    _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }

            public void HydraulicErosion(Types.TileMapCont tileMap,
                float thermalErosionTimeScale = 1f, float thermalErosionRate = 0.15f,
                float talusAngleTangentCoeff = 0.8f, float talusAngleTangentBias = 0.1f,
                float sedimentCapacity = 1f, float maxErosionDepth = 10f,
                float suspensionRate = 0.5f, float depositionRate = 1f, float sedimentSofteningRate = 5f,
                float deltaTime = 1f, float pipeArea = 20f, float pipeLength = 1f / 256f,
                float evaporation = 0.015f, float rainRate = 0.012f,
                Vector2? cellScale = null, int iterations = 1, bool useGPU = true)
            {
                if (cellScale == null)
                    cellScale = Vector2.one;
                if (useGPU)
                    HydraulicErosionGPU(tileMap,
                        thermalErosionTimeScale, thermalErosionRate,
                        talusAngleTangentCoeff, talusAngleTangentBias,
                        sedimentCapacity, maxErosionDepth,
                        suspensionRate, depositionRate, sedimentSofteningRate,
                        deltaTime, pipeArea, pipeLength,
                        evaporation, rainRate,
                        (Vector2) cellScale, iterations);
                else
                    HydraulicErosionCPU(tileMap,
                        thermalErosionTimeScale, thermalErosionRate,
                        talusAngleTangentCoeff, talusAngleTangentBias,
                        sedimentCapacity, maxErosionDepth,
                        suspensionRate, depositionRate, sedimentSofteningRate,
                        deltaTime, pipeArea, pipeLength,
                        evaporation, rainRate,
                        (Vector2) cellScale, iterations);
            }

            private void HydraulicErosionCPU(Types.TileMapCont tileMap,
                float thermalErosionTimeScale, float thermalErosionRate,
                float talusAngleTangentCoeff, float talusAngleTangentBias,
                float sedimentCapacity, float maxErosionDepth,
                float suspensionRate, float depositionRate, float sedimentSofteningRate,
                float deltaTime, float pipeArea, float pipeLength,
                float evaporation, float rainRate,
                Vector2 cellScale, int iterations)
            {

            }
            private void HydraulicErosionGPU(Types.TileMapCont tileMap,
                float thermalErosionTimeScale, float thermalErosionRate,
                float talusAngleTangentCoeff, float talusAngleTangentBias,
                float sedimentCapacity, float maxErosionDepth,
                float suspensionRate, float depositionRate, float sedimentSofteningRate,
                float deltaTime, float pipeArea, float pipeLength,
                float evaporation, float rainRate,
                Vector2 cellScale, int iterations)
            {
                const int pass1KernelIndex = (int) FunctionKernels.HydraulicErosionPass1;
                const int pass2KernelIndex = (int) FunctionKernels.HydraulicErosionPass2;
                const int pass3KernelIndex = (int) FunctionKernels.HydraulicErosionPass3;
                const int pass4KernelIndex = (int) FunctionKernels.HydraulicErosionPass4;
                const int pass5KernelIndex = (int) FunctionKernels.HydraulicErosionPass5;
                const int pass6KernelIndex = (int) FunctionKernels.ThermalErosionPass1;
                const int pass7KernelIndex = (int) FunctionKernels.ThermalErosionPass2;
                const int initKernelIndex    = (int) FunctionKernels.InitErosionTileMap;
                const int updateKernelIndex  = (int) FunctionKernels.UpdateErosionTileMap;

                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);

                float[] cells = tileMap.GetCells();

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetFloat(_ThermalErosionTimeScaleID, thermalErosionTimeScale);
                _FunctionLibrary._ComputeShader.SetFloat(_ThermalErosionRateID, thermalErosionRate);
                _FunctionLibrary._ComputeShader.SetFloat(_TalusAngleTangentCoeffID, talusAngleTangentCoeff);
                _FunctionLibrary._ComputeShader.SetFloat(_TalusAngleTangentBiasID, talusAngleTangentBias);
                _FunctionLibrary._ComputeShader.SetFloat(_SedimentCapacityID, sedimentCapacity);
                _FunctionLibrary._ComputeShader.SetFloat(_MaxErosionDepthID, maxErosionDepth);
                _FunctionLibrary._ComputeShader.SetFloat(_SuspensionRateID, suspensionRate);
                _FunctionLibrary._ComputeShader.SetFloat(_DepositionRateID, depositionRate);
                _FunctionLibrary._ComputeShader.SetFloat(_SedimentSofteningRateID, sedimentSofteningRate);
                _FunctionLibrary._ComputeShader.SetFloat(_DeltaTimeID, deltaTime);
                _FunctionLibrary._ComputeShader.SetFloat(_PipeAreaID, pipeArea);
                _FunctionLibrary._ComputeShader.SetFloat(_PipeLengthID, pipeLength);
                _FunctionLibrary._ComputeShader.SetFloat(_EvaporationID, evaporation);
                _FunctionLibrary._ComputeShader.SetFloat(_RainRateID, rainRate);
                _FunctionLibrary._ComputeShader.SetVector(_CellScaleID, new Vector2(1f, 1f));

                bool bufferFlag = false;
                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);

                // Initialise
                _FunctionLibrary._ComputeShader.SetBuffer(initKernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(initKernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBuffer(initKernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(initKernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);

                _FunctionLibrary._ComputeShader.Dispatch(initKernelIndex, groupsX, groupsY, 1);

                // Iterate
                _FunctionLibrary._ComputeShader.SetBuffer(pass1KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass1KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(pass2KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass2KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(pass3KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass3KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(pass4KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass4KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(pass5KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass5KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(pass6KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass6KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(pass7KernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(pass7KernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);
                
                for (int i = 0; i < iterations; i++)
                {
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass1KernelIndex, groupsX, groupsY, 1);
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass2KernelIndex, groupsX, groupsY, 1);
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass3KernelIndex, groupsX, groupsY, 1);
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass4KernelIndex, groupsX, groupsY, 1);
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass5KernelIndex, groupsX, groupsY, 1);
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass6KernelIndex, groupsX, groupsY, 1);
                    bufferFlag = !bufferFlag;
                    _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                    _FunctionLibrary._ComputeShader.Dispatch(pass6KernelIndex, groupsX, groupsY, 1);
                }
                // ErosionTile[] tiles = new ErosionTile[cells.Length];
                // if (bufferFlag)
                //     _FunctionLibrary._TileMapHydraulic0Buffer.GetData(tiles);
                // else
                //     _FunctionLibrary._TileMapHydraulic1Buffer.GetData(tiles);
                // Debug.Log(tiles[0].tHeight + " " + tiles[0].wHeight + " " + tiles[0].sediment + " " + tiles[0].normal + " " + tiles[0].speed);

                // Finalize
                _FunctionLibrary._ComputeShader.SetBuffer(updateKernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(updateKernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBuffer(updateKernelIndex, _TileMapHydraulic0ID, _FunctionLibrary._TileMapHydraulic0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(updateKernelIndex, _TileMapHydraulic1ID, _FunctionLibrary._TileMapHydraulic1Buffer);

                // bufferFlag = !bufferFlag;
                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                _FunctionLibrary._ComputeShader.Dispatch(updateKernelIndex, groupsX, groupsY, 1);

                if (bufferFlag)
                    _FunctionLibrary._TileMapCont0Buffer.GetData(cells);
                else
                    _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }
        }
    }
}
