// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ToonEffect"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}

		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.7,0.7,0.7,1)
		_SpecularColor("Specular Color", Color) = (0.7,0.7,0.7,1)
		_Glossiness("Glossiness", Float) = 40
        _OutlineColor("Outline Color", Color)=(0,0,0,1)
        _OutlineSize("OutlineSize", Range(1.0,1.5))=1.025
		
	}
	SubShader
	{
		Pass
		{

	
			Tags
			{
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}
			cull back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;	

				SHADOW_COORDS(2)
		
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				//Directional Light
				o.worldNormal = UnityObjectToWorldNormal(v.normal);	
				//Specular Reflection	
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				TRANSFER_SHADOW(o);
	
				return o;
			}
			
			float4 _Color;

			float4 _AmbientColor;

			float4 _SpecularColor;
			float _Glossiness;		

			float4 frag (v2f i) : SV_Target
			{
				float4 sample = tex2D(_MainTex, i.uv);
				//Directional Light
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				float shadow = SHADOW_ATTENUATION(i);

				//Toony Effect on Directional Light
				//returns 0 below 0, returns 1 above 0.01, returns NdotL otherwise.
				float lightIntensity = smoothstep(0, 0.02, NdotL * shadow);	
				//Ambient Light
				float4 light = lightIntensity * _LightColor0;
				//Specular Reflection
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				//Toony Effect on Specular Reflection
				float specularIntensitySmooth = smoothstep(0.005, 0.02, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;				
				//Applying Directional light, Ambient light, Specular reflection 
				return (light + _AmbientColor + specular ) * _Color * sample;
			}
			ENDCG
			

		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		
	    Pass
		{
			cull front
		    CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _OutlineColor;
            float _OutlineSize;

            struct appdata
            {
                float4 vertex:POSITION;
            };
            struct v2f
            {
                float4 vertex:SV_POSITION;
            };
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex=UnityObjectToClipPos(v.vertex*_OutlineSize);

                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

	}
	
	
}