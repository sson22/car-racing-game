Shader "Custom/BlinnPhong"

{
	Properties
	{
		_DarkColor("Dark Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}

		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.7,0.7,0.7,1)
		_SpecularColor("Specular Color", Color) = (0.7,0.7,0.7,1)
		_Glossiness("Glossiness", Float) = 40
		
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
			
			float4 Dark;

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

				//Directional Light
				//returns 0 below 0, returns 1 above 0.01, returns NdotL otherwise.
				float lightIntensity = NdotL * shadow;	
				//Ambient Light
				float4 light = lightIntensity * _LightColor0;
				//Specular Reflection
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				//Specular Reflection
				float4 specular = specularIntensity * _SpecularColor;				
				//Applying Directional light, Ambient light, Specular reflection 
				return (light  + specular ) * Dark * sample;
			}
			ENDCG
			

		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		
	}
}