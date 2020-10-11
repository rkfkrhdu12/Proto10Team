Shader "Unlit/CelShader2"
{
   Properties
   {
      _ASEOutlineColor( "Outline Color", Color ) = (0.1226415,0.1226415,0.1226415,0)
      _ASEOutlineWidth( "Outline Width", Float ) = 0.02
      _normal("normal", 2D) = "bump" {}
      _albedo("albedo", 2D) = "white" {}
      _Color0("Color 0", Color) = (0,0,0,0)
      [HideInInspector] _texcoord( "", 2D ) = "white" {}
      [HideInInspector] __dirty( "", Int ) = 1
   }

   SubShader
   {
      Tags{ }
      Cull Front
      CGPROGRAM
      #pragma target 3.0
      #pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc



      struct Input {
         half filler;
      };
      UNITY_INSTANCING_BUFFER_START(toon)
      UNITY_DEFINE_INSTANCED_PROP( float4, _ASEOutlineColor )
#define _ASEOutlineColor_arr toon
      UNITY_DEFINE_INSTANCED_PROP( float, _ASEOutlineWidth )
#define _ASEOutlineWidth_arr toon
      UNITY_INSTANCING_BUFFER_END(toon)
      void outlineVertexDataFunc( inout appdata_full v, out Input o )
      {
         UNITY_INITIALIZE_OUTPUT( Input, o );
         v.vertex.xyz += ( v.normal * UNITY_ACCESS_INSTANCED_PROP(_ASEOutlineWidth_arr, _ASEOutlineWidth) );
      }
      inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
      void outlineSurf( Input i, inout SurfaceOutput o )
      {
         o.Emission = UNITY_ACCESS_INSTANCED_PROP(_ASEOutlineColor_arr, _ASEOutlineColor).rgb;
         o.Alpha = 1;
      }
      ENDCG


      Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
      Cull Back
      CGPROGRAM
      #include "UnityPBSLighting.cginc"
      #pragma target 3.0
      #pragma multi_compile_instancing
      #pragma surface surf StandardCustomLighting keepalpha addshadow fullforwardshadows exclude_path:deferred
      struct Input
      {
         float2 uv_texcoord;
      };

      struct SurfaceOutputCustomLightingCustom
      {
         half3 Albedo;
         half3 Normal;
         half3 Emission;
         half Metallic;
         half Smoothness;
         half Occlusion;
         half Alpha;
         Input SurfInput;
         UnityGIInput GIData;
      };

      uniform sampler2D _normal;
      uniform sampler2D _albedo;

      UNITY_INSTANCING_BUFFER_START(toon)
         UNITY_DEFINE_INSTANCED_PROP(float4, _albedo_ST)
#define _albedo_ST_arr toon
         UNITY_DEFINE_INSTANCED_PROP(float4, _Color0)
#define _Color0_arr toon
      UNITY_INSTANCING_BUFFER_END(toon)

      inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
      {
         UnityGIInput data = s.GIData;
         Input i = s.SurfInput;
         half4 c = 0;
         float4 _albedo_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_albedo_ST_arr, _albedo_ST);
         float2 uv_albedo = i.uv_texcoord * _albedo_ST_Instance.xy + _albedo_ST_Instance.zw;
         float4 _Color0_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color0_arr, _Color0);
         float4 albedo37 = ( tex2D( _albedo, uv_albedo ) * _Color0_Instance );
         c.rgb = albedo37.rgb;
         c.a = 1;
         return c;
      }

      inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
      {
         s.GIData = data;
      }

      void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
      {
         o.SurfInput = i;
      }

      ENDCG
   }
   Fallback "Diffuse"
   CustomEditor "ASEMaterialInspector"
}
