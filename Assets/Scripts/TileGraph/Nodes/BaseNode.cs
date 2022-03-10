using System;
using UnityEngine;
using XNode;

namespace TileGraph.Nodes
{
    /// <summary> Base node class containing operations and parameters common
    /// to all CAGraph nodes. </summary>
    public abstract class BaseNode : Node
    {
        [Serializable]
        protected struct GPUOption
        {
            public bool isEnabled;
            public bool isOverridden;
        }
        [SerializeField, HideInInspector]
        private GPUOption _GPUEnabled = new GPUOption {isEnabled = true, isOverridden = false};
        protected bool GPUEnabled
        {
            get
            {
                if (_GPUEnabled.isOverridden)
                    return _GPUEnabled.isEnabled;
                else
                    return _Graph.GPUEnabledGlobal;
            }
            set {
                _GPUEnabled.isEnabled = value;
                _GPUEnabled.isOverridden = true;
            }
        }
        public bool isGPUEnabled {get {return _GPUEnabled.isEnabled;}}
        public bool isGPUOverriden {get {return _GPUEnabled.isOverridden;}}

        protected TileGraph _Graph;

        protected override void Init()
        {
            base.Init();
            _Graph = (TileGraph) graph;
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            if (ReferenceEquals(to.node, this) && from.ValueType != to.ValueType)
            {
                to.ClearConnections();
                Debug.LogWarning(
                    string.Format("Output type '{0}' does not match expected input type '{1}'",
                                  from.ValueType.Name,
                                  to.ValueType.Name)
                );
            }
        }

        /// <summary> Check the given parameters to identify if a change has
        /// been made requiring an update to the output TileMap. If a change
        /// has been detected in the TileMap at
        /// <paramref name="inputPortName" /> the <paramref name="outBuffer" />
        /// and <paramref name="inBuffer" /> will be set accordingly (either to
        /// null and 0 respectively if the input is null, or to a clone of the
        /// input and the id of the input respectively). In the event of a
        /// change to a non-null value a call to UpdateTileMapOutput will be
        /// made with <paramref name="outputPortName" /> as the parameter.
        /// </summary>
        /// <param name="inputPortName"> Name of the input port receiving the
        /// input value TileMap. </param>
        /// <param name="outputPortName"> Name of the port the output TileMap is
        /// to be sent to. </param>
        /// <param name="outBuffer"> Reference to the buffer used to store the
        /// output TileMap. </param>
        /// <param name="inBuffer"> Reference to the buffer used to store the
        /// ID of the input TileMap. </param>
        /// <param name="parameterChanged"> <c>true</c> if a value relevant to
        /// the output has been changed, otherwise <c>false</c>. </param>
        /// <typeparam name="MTypeIn"> Type of TileMap being input. Assumed to
        /// be the output type as well, if not specified.
        /// </typeparam>
        protected void GetTileMapInput<MTypeIn>(string inputPortName, string outputPortName,
                                                ref MTypeIn outBuffer, ref long inBuffer,
                                                bool parameterChanged = false)
            where MTypeIn : Types.TileMap
        {
            MTypeIn tileMap = GetInputValue<MTypeIn>(inputPortName);
            if (tileMap == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || tileMap.GetID() != inBuffer)
            {
                inBuffer = tileMap.GetID();
                outBuffer = (MTypeIn) tileMap.Clone();
                UpdateTileMapOutput(outputPortName);
            }
            else if (parameterChanged)
            {
                outBuffer = (MTypeIn) tileMap.Clone();
                UpdateTileMapOutput(outputPortName);
            }
        }
        /// <summary> Check the given parameters to identify if a change has
        /// been made requiring an update to the output TileMap. If a change
        /// has been detected in the TileMap at
        /// <paramref name="inputPortName" /> the <paramref name="outBuffer" />
        /// and <paramref name="inBuffer" /> will be set accordingly (either to
        /// null and 0 respectively if the input is null, or the
        /// <paramref name="inBuffer" /> will be set to the id of the input).
        /// In the event of a change to a non-null value a call to
        /// UpdateTileMapOutput will be made with
        /// <paramref name="outputPortName" /> as the parameter. </summary>
        /// <param name="inputPortName"> Name of the input port receiving the
        /// input value TileMap. </param>
        /// <param name="outputPortName"> Name of the port the output TileMap is
        /// to be sent to. </param>
        /// <param name="outBuffer"> Reference to the buffer used to store the
        /// output TileMap. </param>
        /// <param name="inBuffer"> Reference to the buffer used to store the
        /// ID of the input TileMap. </param>
        /// <param name="parameterChanged"> <c>true</c> if a value relevant to
        /// the output has been changed, otherwise <c>false</c>. </param>
        /// <typeparam name="MTypeIn"> Type of TileMap being input.
        /// </typeparam>
        /// <typeparam name="MTypeOut"> Type of TileMap to be output.
        /// </typeparam>
        protected void GetTileMapInput<MTypeIn, MTypeOut>(string inputPortName, string outputPortName,
                                                ref MTypeOut outBuffer, ref long inBuffer,
                                                bool parameterChanged = false)
            where MTypeIn : Types.TileMap where MTypeOut : Types.TileMap
        {
            MTypeIn tileMap = GetInputValue<MTypeIn>(inputPortName);
            if (tileMap == null)
            {
                inBuffer = 0L;
                outBuffer = null;
            }
            else if (outBuffer == null || tileMap.GetID() != inBuffer)
            {
                inBuffer = tileMap.GetID();
                UpdateTileMapOutput(outputPortName);
            }
            else if (parameterChanged)
            {
                UpdateTileMapOutput(outputPortName);
            }
        }

        /// <summary> Update the contents of the TileMap buffer being sent to
        /// the output port at <paramref name="portName" /> </summary>
        protected virtual void UpdateTileMapOutput(string portName)
        {
            
        }

        public void ToggleGPUEnabled()
        {
            GPUEnabled = !GPUEnabled;
        }
        public void ResetGPUEnabled()
        {
            _GPUEnabled.isOverridden = false;
        }
    }
}
