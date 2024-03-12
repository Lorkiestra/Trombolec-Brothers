Shader "Custom/ScreenSpaceLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Resolution("Resolution", vector) = (0, 0, 0, 0)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf ScreenSpaceLambert vertex:vert fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 4.0

        sampler2D _MainTex;

        half4 LightingScreenSpaceLambert(SurfaceOutput s, half3 lightDir, half atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            NdotL /= s.Specular;
            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            c.a = s.Alpha;
            return c;
        }

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
            float3 worldNormal;
            float3 tspace0;
            float3 tspace1;
            float3 tspace2;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _Resolution;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

            void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            //yeah vr specialties stuff again
            o.screenPos = ComputeScreenPos(UnityObjectToClipPos(v.vertex));
            
            //normals extraction
            //float4 pos = UnityObjectToClipPos(v.vertex);
            //float4 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            float3 wNormal = UnityObjectToWorldNormal(v.normal);
            float3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
            // compute bitangent from cross product of normal and tangent
            half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
            half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
            // output the tangent space matrix
            o.tspace0 = float3(wTangent.x, wBitangent.x, wNormal.x);
            o.tspace1 = float3(wTangent.y, wBitangent.y, wNormal.y);
            //o.tspace2 = float3(wTangent.z, wBitangent.z, wNormal.z);
            //o.uv_normal = v.uv_normal;
            o.worldNormal = wNormal;

        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 screenUv = IN.screenPos.xy / IN.screenPos.w;
            screenUv.x *= _Resolution.x;
            screenUv.x = round(screenUv.x);
            screenUv.x /= _Resolution.x;
            screenUv.y *= _Resolution.y;
            screenUv.y = round(screenUv.y);
            screenUv.y /= _Resolution.y;

            // sample the normal map, and decode from the Unity encoding
            //float3 tnormal = UnpackNormal(tex2D(_NormalMap, i.uv_normal));
            // transform normal from tangent to world space
            //float3 worldNormal;
            //worldNormal.x = dot(i.tspace0, tnormal);
            //worldNormal.y = dot(i.tspace1, tnormal);
            //worldNormal.z = dot(i.tspace2, tnormal);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Specular = length(screenUv);
            o.Alpha = c.a;
            o.Normal = (0, 0, 1.0);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
