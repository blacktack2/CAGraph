#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
    StructuredBuffer<int> _Cells;
    StructuredBuffer<float3> _Positions;
#endif

void ConfigureProcedural()
{
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        bool cell = _Cells[unity_InstanceID];
        float3 position = _Positions[unity_InstanceID];

        unity_ObjectToWorld = 0.0;
        unity_ObjectToWorld._m03_m13_m23_m33 = float4(position.xy, 0.0, 1.0);
        unity_ObjectToWorld._m00_m11_m22 = 1;
    #endif
}

void ShaderGraphFunction_float(float3 In, out bool OutCell, out float3 OutPos)
{
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        OutCell = _Cells[unity_InstanceID];
    #else
        OutCell = false;
    #endif
    OutPos = In;
}

void ShaderGraphFunction_half(float3 In, out bool OutCell, out float3 OutPos)
{
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        OutCell = _Cells[unity_InstanceID];
    #else
        OutCell = false;
    #endif
    OutPos = In;
}