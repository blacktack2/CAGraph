using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAGraph.Utilities
{
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
            _Cells0Buffer = new ComputeBuffer(Types.Matrix.maxMatrixSize * Types.Matrix.maxMatrixSize, sizeof(int));
            _Cells1Buffer = new ComputeBuffer(Types.Matrix.maxMatrixSize * Types.Matrix.maxMatrixSize, sizeof(int));
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

        public void IterateCells(Types.Matrix matrix, int[] rules, int iterations)
        {
            int kernelIndex = 0;
            bool bufferFlag = false;

            int[] cells = matrix.GetCells();

            _Cells0Buffer.SetData(cells);
            _Cells1Buffer.SetData(cells);
            _LifeRulesBuffer.SetData(rules);

            _ComputeShader.SetInt(_ScaleXID, matrix.width);
            _ComputeShader.SetInt(_ScaleYID, matrix.height);
            _ComputeShader.SetBuffer(kernelIndex, _Cells0ID, _Cells0Buffer);
            _ComputeShader.SetBuffer(kernelIndex, _Cells1ID, _Cells1Buffer);
            _ComputeShader.SetBuffer(kernelIndex, _LifeRulesID, _LifeRulesBuffer);

            int groupsX = Mathf.CeilToInt(matrix.width / 8f);
            int groupsY = Mathf.CeilToInt(matrix.height / 8f);
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
            matrix.SetCells(cells);
        }
    }
}
