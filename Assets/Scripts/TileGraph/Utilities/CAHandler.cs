using UnityEngine;

namespace TileGraph.Utilities
{
    /// <summary> Handler class for applying Cellular Automata operations to
    /// matrices. </summary>
    public class CAHandler
    {
        private static readonly int
            _Cells0ID = Shader.PropertyToID("_Cells0"),
            _Cells1ID = Shader.PropertyToID("_Cells1"),
            _LifeRulesID = Shader.PropertyToID("_LifeRules"),
            _BufferFlagID = Shader.PropertyToID("_BufferFlag"),
            _ScaleXID = Shader.PropertyToID("_ScaleX"),
            _ScaleYID = Shader.PropertyToID("_ScaleY");
        
        private ComputeShader _ComputeShader;
            
        private ComputeBuffer _Cells0Buffer;
        private ComputeBuffer _Cells1Buffer;
        private ComputeBuffer _LifeRulesBuffer;

        public CAHandler(ComputeShader computeShader)
        {
            _ComputeShader = computeShader;
        }

        public void Enable()
        {
            _Cells0Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(int));
            _Cells1Buffer = new ComputeBuffer(Types.TileMapBool.maxTileMapSize * Types.TileMapBool.maxTileMapSize, sizeof(int));
            _LifeRulesBuffer = new ComputeBuffer(18, sizeof(int));
        }

        public void Disable()
        {
            _Cells0Buffer.Dispose();
            _Cells1Buffer.Dispose();
            _LifeRulesBuffer.Dispose();

            _Cells0Buffer = null;
            _Cells1Buffer = null;
            _LifeRulesBuffer = null;
        }

        /// <summary> Run a lifelike CA simulation on
        /// <paramref name="tileMap" /> for a given set of rules and
        /// number of itetrations. </summary>
        /// <param name="tileMap"> <paramref name="TileMapBool" /> to run the simulation on.
        /// </param>
        /// <param name="rules"> Array of 18 0-1 integers representing the
        /// lifelike CA rules. Indexes 0-8 represent the birth rules for a
        /// number of neighbours corresponding to each index. Likewise with
        /// indexes 9-17 representing the survival rules. 0 will die/stay dead,
        /// 1 will survive/be born </param>
        /// <param name="iterations"> Number of iterations to run the simulation for. </param>
        public void IterateCells(Types.TileMapBool tileMap, int[] rules, int iterations)
        {
            int kernelIndex = 0;
            bool bufferFlag = false;

            int[] cells = tileMap.GetCells();

            _Cells0Buffer.SetData(cells);
            _Cells1Buffer.SetData(cells);
            _LifeRulesBuffer.SetData(rules);

            _ComputeShader.SetInt(_ScaleXID, tileMap.width);
            _ComputeShader.SetInt(_ScaleYID, tileMap.height);
            _ComputeShader.SetBuffer(kernelIndex, _Cells0ID, _Cells0Buffer);
            _ComputeShader.SetBuffer(kernelIndex, _Cells1ID, _Cells1Buffer);
            _ComputeShader.SetBuffer(kernelIndex, _LifeRulesID, _LifeRulesBuffer);

            int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
            int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
            // Each iteration in a CA must run sequentially, but individual
            // cells in an iteration can run in parallel
            for (int i = 0; i < iterations; i++)
            {
                bufferFlag = !bufferFlag;
                _ComputeShader.SetBool(_BufferFlagID, bufferFlag);

                _ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);
            }

            if (bufferFlag)
                _Cells0Buffer.GetData(cells);
            else
                _Cells1Buffer.GetData(cells);
            tileMap.SetCells(cells);
        }
    }
}
