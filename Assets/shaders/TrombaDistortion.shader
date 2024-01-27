Shader "Custom/TrombaDistortion"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _NoiseScale("NoiseScale", vector) = (0.1, 0.1, 0.1, 3)
        _PoundScale("PoundScale", vector) = (1, 2.5, 1.3, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "UsefulCalculations.cginc"

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        float4 _NoiseScale;
        float4 _PoundScale;
        uniform float4 GroundpoundPos1;
        uniform float4 GroundpoundPos2;
        uniform float PoundDist1;
        uniform float PoundDist2;
        uniform float4 TrombaPos1;
        uniform float4 TrombaPos2;
        uniform float SuccPower1;
        uniform float SuccPower2;
        uniform float SuccSpeed1;
        uniform float SuccSpeed2;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            //o.worldPos = float4(mul(unity_ObjectToWorld, v.vertex).xyz, 1);
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;


            //tromba push / pull
            float3 centeredPos = worldPos - TrombaPos1;
            float dist = length(centeredPos);

            float3 timedPos = centeredPos;
            timedPos += _Time * SuccSpeed1;
            dist -= (GradientNoise(timedPos / _NoiseScale.xyz) + 1) / 2 * SuccPower1;
            dist = max(0, dist);
            //regaining world pos
            worldPos = TrombaPos1 + normalize(centeredPos) * dist;

            
            //tromba push / pull
            centeredPos = worldPos - TrombaPos2;
            dist = length(centeredPos);

            timedPos = centeredPos;
            timedPos += _Time * SuccSpeed2;
            dist -= (GradientNoise(timedPos / _NoiseScale.xyz) + 1) / 2 * SuccPower2;
            dist = max(0, dist);
            //regaining world pos
            worldPos = TrombaPos2 + normalize(centeredPos) * dist;

            //groundpound
            centeredPos = worldPos - GroundpoundPos1.xyz;
            float dist2d = length(centeredPos.xz);
            float waveDist = (dist2d - PoundDist1) * _PoundScale.x;
            //make only singular wave
            if (abs(waveDist) < 3.14)
            {
                worldPos.y += max(0, _PoundScale.y * sin(waveDist) / max(1, dist2d * _PoundScale.z));
            }

            //same for second brother
            centeredPos = worldPos - GroundpoundPos2.xyz;
            dist2d = length(centeredPos.xz);
            waveDist = (dist2d - PoundDist2) * _PoundScale.x;
            //make only singular wave
            if (abs(waveDist) < 3.14)
            {
                worldPos.y += max(0, _PoundScale.y * sin(waveDist) / max(1, dist2d * _PoundScale.z));
            }

            //world to vertex pos:
            v.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1));

        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
