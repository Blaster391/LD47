// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/DestructableShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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
        #pragma surface surf Standard vertex:vert // fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        struct VS_Input
        {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            uint   id : SV_VertexID;
        };

 

        int _vertexCount;
        float _destructTime;
        float _lowEndTime;
        float _highEndTime;
        float3 _collisionPosition;

        void vert(inout VS_Input v)
        {
            if (_destructTime >= _highEndTime)
            {
                v.vertex = float4(0, 0, 0, 0);
            }
            else if (_destructTime > 0.0f)
            {
               
                //prop = 1.0f - prop; 
                //v.vertex.xyz += v.normal * float3(prop * v.vertex.x,prop * v.vertex.y,prop * v.vertex.z);*/
                
                /*uint scrambledId = fmod(v.id + 2731u, 983u);
                float floatId = float(scrambledId);
                float modifier = (floatId / 983.0f);*/

                float floatId = float(v.id);
                float modifier = (floatId / float(_vertexCount));

                float timeDiff = _highEndTime - _lowEndTime;
                float myTime = modifier * timeDiff;
                float prop = (_destructTime / myTime);
                prop = min(prop, 1.0f);

                float4 collisionPointLocal = mul(unity_WorldToObject, float4(_collisionPosition, 1.0f));
                float3 lerpPoint = lerp(v.vertex.xyz, collisionPointLocal, prop);
                v.vertex.xyz = lerpPoint;
            }
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

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
