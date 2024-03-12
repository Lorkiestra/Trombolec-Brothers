Shader "Custom/worldNormal"
{
    Properties{
    }
    SubShader
    {
    LOD 200

    CGPROGRAM
    #pragma surface surf Lambert

    struct Input
     {
INTERNAL_DATA
float3 worldRefl;
};


void surf(Input IN, inout SurfaceOutput o)
 {

    //trick to compute world space normal
    IN.worldRefl = normalize(IN.worldRefl);
    float3 worldRefl = WorldReflectionVector(IN, o.Normal);
    float3 worldNormal = normalize(worldRefl - IN.worldRefl);

    o.Albedo = worldNormal;
    //o.Emission = worldNormal;
    o.Normal = float3(0, 0, 1.0);
    }
        ENDCG
    }
    FallBack "Diffuse"
}
