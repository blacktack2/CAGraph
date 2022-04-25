using System;
using System.Collections.Generic;
using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Erosion : SubLibrary
        {
            public enum Algorithm {
                Hydraulic,
                Fluvial,
                Thermal
            }
            [Serializable]
            public struct ErosionPass
            {
                public Algorithm algorithm;
                public int repeats;
                // Hydraulic parameters
                public float terrainHardness;
                public float sedimentHardness;
                public float depositionRate;
                public float rainRate;
                public float rainAmount;
                // Fluvial parameters
                // Thermal parameters
                public float maxSlope;
                public float thermalRate;

                public override bool Equals(object obj)
                {
                    if (!(obj is ErosionPass) || obj == null)
                        return false;
                    ErosionPass other = (ErosionPass)(obj as ErosionPass?);
                    if (other.algorithm != algorithm || other.repeats != repeats
                        || other.terrainHardness != terrainHardness || other.sedimentHardness != sedimentHardness || other.depositionRate != depositionRate
                        || other.rainRate != rainRate || other.rainAmount != rainAmount
                        || other.maxSlope != maxSlope || other.thermalRate != thermalRate)
                        return false;
                    return true;
                }

                public override int GetHashCode()
                {
                    HashCode hash = new HashCode();
                    hash.Add(algorithm);
                    hash.Add(repeats);
                    hash.Add(terrainHardness);
                    hash.Add(sedimentHardness);
                    hash.Add(depositionRate);
                    hash.Add(rainRate);
                    hash.Add(rainAmount);
                    hash.Add(maxSlope);
                    hash.Add(thermalRate);
                    return hash.ToHashCode();
                }
            }
            public struct ErosionTile
            {
                public float landH;
                public float sedH;
                public float waterVH;
                public float waterVF;
            }

            public Erosion(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            public void Combined(Types.TileMapCont tileMap, List<ErosionPass> passes, int iterations, bool useGPU = true)
            {
                if (useGPU)
                    CombinedGPU(tileMap, passes, iterations);
                else
                    CombinedCPU(tileMap, passes, iterations);
            }
            private void CombinedCPU(Types.TileMapCont tileMap, List<ErosionPass> passes, int iterations)
            {

            }
            private void CombinedGPU(Types.TileMapCont tileMap, List<ErosionPass> passes, int iterations)
            {
                const int hydraulicKernel = (int) FunctionKernels.HydraulicErosion;
                const int fluvialKernel   = (int) FunctionKernels.FluvialErosion;
                const int thermalKernel   = (int) FunctionKernels.ThermalErosion;
                
                float[] cells = tileMap.GetCells();
                ErosionTile[] tiles = new ErosionTile[cells.Length];
                for (int i = 0; i < tiles.Length; i++)
                    tiles[i] = new ErosionTile() {landH = cells[i], sedH = 0f, waterVH = 0f, waterVF = 0f};
                
                _FunctionLibrary._ComputeShader.SetBuffer(hydraulicKernel, _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(hydraulicKernel, _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(fluvialKernel,   _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(fluvialKernel,   _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                
                _FunctionLibrary._ComputeShader.SetBuffer(thermalKernel,   _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(thermalKernel,   _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);

                _FunctionLibrary._TileMapErosion0Buffer.SetData(tiles);
                _FunctionLibrary._TileMapErosion1Buffer.SetData(tiles);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);

                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                bool bufferFlag = false;
                int counter = 0;
                for (int i = 0; i < iterations; i++)
                { 
                    foreach (ErosionPass pass in passes)
                    {
                        int kernelIndex;
                        switch (pass.algorithm)
                        {
                            case Algorithm.Hydraulic: default:
                                _FunctionLibrary._ComputeShader.SetFloat(_TerrainHardnessID,  pass.terrainHardness);
                                _FunctionLibrary._ComputeShader.SetFloat(_SedimentHardnessID, pass.sedimentHardness);
                                _FunctionLibrary._ComputeShader.SetFloat(_DepositionRateID,   pass.depositionRate);
                                _FunctionLibrary._ComputeShader.SetFloat(_RainRateID,         pass.rainRate);
                                _FunctionLibrary._ComputeShader.SetFloat(_RainAmountID,       pass.rainAmount);
                                kernelIndex = hydraulicKernel;
                                break;
                            case Algorithm.Fluvial:
                                kernelIndex = fluvialKernel;
                                break;
                            case Algorithm.Thermal:
                                _FunctionLibrary._ComputeShader.SetFloat(_MaxSlopeID,    pass.maxSlope);
                                _FunctionLibrary._ComputeShader.SetFloat(_ThermalRateID, pass.thermalRate);
                                kernelIndex = thermalKernel;
                                break;
                        }
                        for (int r = 0; r < pass.repeats; r++)
                        {
                            _FunctionLibrary._ComputeShader.SetInt(_IterationID, counter++);
                            bufferFlag = !bufferFlag;
                            _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, bufferFlag);
                            _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);
                        }
                    }
                }

                if (bufferFlag)
                    _FunctionLibrary._TileMapErosion0Buffer.GetData(tiles);
                else
                    _FunctionLibrary._TileMapErosion1Buffer.GetData(tiles);
                
                for (int i = 0; i < cells.Length; i++)
                    cells[i] = tiles[i].landH + tiles[i].sedH;
                tileMap.SetCells(cells);
            }

            public void Hydraulic(Types.TileMapCont tileMap, int iterations = 1,
                float terrainHardness = 1f, float sedimentHardness = 1f, float depositionRate = 1f,
                float rainRate = 0.5f, float rainAmount = 1f,
                bool useGPU = true)
            {
                if (useGPU)
                    HydraulicGPU(tileMap, iterations, terrainHardness, sedimentHardness, depositionRate, rainRate, rainAmount);
                else
                    HydraulicCPU(tileMap, iterations, terrainHardness, sedimentHardness, depositionRate, rainRate, rainAmount);
            }
            private void HydraulicCPU(Types.TileMapCont tileMap, int iterations,
                float terrainHardness, float sedimentHardness, float depositionRate,
                float rainRate, float rainAmount)
            {

            }
            private void HydraulicGPU(Types.TileMapCont tileMap, int iterations,
                float terrainHardness, float sedimentHardness, float depositionRate,
                float rainRate, float rainAmount)
            {
                const int kernelIndex = (int) FunctionKernels.HydraulicErosion;

                float[] cells = tileMap.GetCells();
                ErosionTile[] tiles = new ErosionTile[cells.Length];
                for (int i = 0; i < tiles.Length; i++)
                    tiles[i] = new ErosionTile() {landH = cells[i], sedH = 0f, waterVH = 0f, waterVF = 0f};
                
                _FunctionLibrary._ComputeShader.SetFloat(_TerrainHardnessID, terrainHardness);
                _FunctionLibrary._ComputeShader.SetFloat(_SedimentHardnessID, sedimentHardness);
                _FunctionLibrary._ComputeShader.SetFloat(_DepositionRateID, depositionRate);
                _FunctionLibrary._ComputeShader.SetFloat(_RainRateID, rainRate);
                _FunctionLibrary._ComputeShader.SetFloat(_RainAmountID, rainAmount);

                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                
                _FunctionLibrary._TileMapErosion0Buffer.SetData(tiles);
                _FunctionLibrary._TileMapErosion1Buffer.SetData(tiles);
                
                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);

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
                    cells[i] = tiles[i].landH + tiles[i].sedH;
                tileMap.SetCells(cells);
            }

            public void Fluvial(Types.TileMapCont tileMap, int iterations = 1, bool useGPU = true)
            {
                if (useGPU)
                    FluvialGPU(tileMap, iterations);
                else
                    FluvialCPU(tileMap, iterations);
            }
            private void FluvialCPU(Types.TileMapCont tileMap, int iterations)
            {

            }
            private void FluvialGPU(Types.TileMapCont tileMap, int iterations)
            {
                const int kernelIndex = (int) FunctionKernels.FluvialErosion;

                float[] cells = tileMap.GetCells();
                ErosionTile[] tiles = new ErosionTile[cells.Length];
                for (int i = 0; i < tiles.Length; i++)
                    tiles[i] = new ErosionTile() {landH = cells[i], sedH = 0f, waterVH = 0f, waterVF = 0f};
                
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                
                _FunctionLibrary._TileMapErosion0Buffer.SetData(tiles);
                _FunctionLibrary._TileMapErosion1Buffer.SetData(tiles);
                
                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);

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

            public void Thermal(Types.TileMapCont tileMap, int iterations = 1,
                float maxSlope = 3.6f, float thermalRate = 0.146f, bool useGPU = false)
            {
                if (useGPU)
                    ThermalGPU(tileMap, iterations, maxSlope, thermalRate);
                else
                    ThermalCPU(tileMap, iterations, maxSlope, thermalRate);
            }
            private void ThermalCPU(Types.TileMapCont tileMap, int iterations, float maxSlope, float thermalRate)
            {

            }
            private void ThermalGPU(Types.TileMapCont tileMap, int iterations, float maxSlope, float thermalRate)
            {
                const int kernelIndex = (int) FunctionKernels.ThermalErosion;

                float[] cells = tileMap.GetCells();
                ErosionTile[] tiles = new ErosionTile[cells.Length];
                for (int i = 0; i < tiles.Length; i++)
                    tiles[i] = new ErosionTile() {landH = cells[i], sedH = 0f, waterVH = 0f, waterVF = 0f};
                
                _FunctionLibrary._ComputeShader.SetFloat(_MaxSlopeID, maxSlope);
                _FunctionLibrary._ComputeShader.SetFloat(_ThermalRateID, thermalRate);

                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion0ID, _FunctionLibrary._TileMapErosion0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapErosion1ID, _FunctionLibrary._TileMapErosion1Buffer);
                
                _FunctionLibrary._TileMapErosion0Buffer.SetData(tiles);
                _FunctionLibrary._TileMapErosion1Buffer.SetData(tiles);
                
                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);

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
                    cells[i] = tiles[i].landH + tiles[i].sedH;
                tileMap.SetCells(cells);
            }
        }
    }
}
