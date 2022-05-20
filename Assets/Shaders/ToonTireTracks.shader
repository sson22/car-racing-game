Shader "Custom/ToonSnowTracks"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)

        _FloorColour ("Floor Color", Color) = (1,1,1,1)
        _FloorTex ("Floot (RGB)", 2D) = "white" {}
        _TrackColour ("Track Color", Color) = (1,1,1,1)
        _TrackTex ("Track (RGB)", 2D) = "white" {}
        _Splat ("SplatMap", 2D) = "black" {}

		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.7,0.7,0.7,1)
		_SpecularColor("Specular Color", Color) = (0.7,0.7,0.7,1)
		_Glossiness("Glossiness", Float) = 40
		_RimColor("Rim Color", Color) = (1,1,1,1)
		//Width of Rim on Rim Lighting Effect 
		_RimAmount("Rim Amount", Range(0, 1)) = 0.7

        _TrackThreshold("Tracks Threshold", Range(0, 1)) = 0.5

        
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
                float2 uv_TrackTex: TEXCOORD1;
                float2 uv_FloorTex : TEXCOORD2;
                float2 uv_Splat : TEXCOORD3;
                float3 viewDir : TEXCOORD4;	

				SHADOW_COORDS(5)
		
			};

            sampler2D _Splat;
            fixed4 _Splat_ST;
            sampler2D _TrackTex;
            fixed4 _TrackColour;
            fixed4 _TrackTex_ST;
            sampler2D _FloorTex;
            fixed4 _FloorTex_ST;
            fixed4 _FloorColour;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				//Directional Light
				o.worldNormal = UnityObjectToWorldNormal(v.normal);	
				//Specular Reflection	
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _TrackTex);

                o.uv_TrackTex= TRANSFORM_TEX(v.uv, _TrackTex);
                o.uv_FloorTex = TRANSFORM_TEX(v.uv, _FloorTex);
                o.uv_Splat = TRANSFORM_TEX(v.uv, _Splat);

				TRANSFER_SHADOW(o);
	
				return o;
			}
			
			float4 _Color;

			float4 _AmbientColor;

			float4 _SpecularColor;
			float _Glossiness;		

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;	

            float _TrackThreshold;

			float4 frag (v2f i) : SV_Target
			{
				//Directional Light
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				float shadow = SHADOW_ATTENUATION(i);
				

				//Toony Effect on Directional Light
				//returns 0 below 0, returns 1 above 0.01, returns NdotL otherwise.
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);	
				//Ambient Light
				float4 light = lightIntensity * _LightColor0;
				//Specular Reflection
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				//Toony Effect on Specular Reflection
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;				

				//Rim Lighting
				//Rim of the object is defined as surfaces that are facing away from the camera.
				//Take the dot product of normal and view direction, and invert it
				float rimDot = 1 - dot(viewDir, normal);
				//Toony Effect on Rim Lighting
				float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
				float4 rim = rimIntensity * _RimColor;

                half amount = tex2Dlod(_Splat, float4(i.uv_Splat,0,0)).r;

                fixed4 c = amount > _TrackThreshold ? _FloorColour : (_TrackColour);

				float4 sample = tex2D(_TrackTex, i.uv);
				//Applying Directional light, Ambient light, Specular reflection, and Rim Lighting 
				return (light + _AmbientColor + specular + rim) * c;
			}
			ENDCG
			

		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		
	}
}