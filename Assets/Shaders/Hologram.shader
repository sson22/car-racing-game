Shader "Unlit/Hologram"
{

    Properties{
        _RimColour ("Rim Colour", Color) = (1,0,0,1)
        _RimPower("Rim Power", Range(1,10)) = 1.0

    }
    SubShader{
        Tags{"Queue" = "Transparent"}
        //Deletes Hologram outlines of all inner parts
        //2 Pass strategy
        Pass{
            ZWrite On
            ColorMask 0
        }

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade
        struct Input{
            float3 viewDir;
        };

        float4 _RimColour = (1,0,0,1);
        float _RimPower = 1.0;
        
        void surf(Input IN, inout SurfaceOutput o){
            float rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColour.rgb * pow(rim, _RimPower) * 10;
            //Added this part to change the Alpha value
            float time = cos(_Time.y)+1;
            rim = pow(rim, _RimPower*time);
            o.Alpha = rim;
           
            
        }
        ENDCG
    }
    Fallback "Diffuse"
}
