Shader "Custom/GradientSkyboxShader" {
    Properties {
        _TopColor ("Top Color", Color) = (1, 1, 1, 1)
        _MiddleColor ("Middle Color", Color) = (0.5, 0.5, 0.5, 1)
        _BottomColor ("Bottom Color", Color) = (0, 0, 0, 1)
        _TopMiddlePosition ("Top-Middle Position", Range(0, 1)) = 0.5
        _MiddleBottomPosition ("Middle-Bottom Position", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float _TopMiddlePosition;
        float _MiddleBottomPosition;
        float3 _TopColor;
        float3 _MiddleColor;
        float3 _BottomColor;

        void surf (Input IN, inout SurfaceOutput o) {
            float gradientPosition = IN.worldNormal.y * 0.5 + 0.5;

            if (gradientPosition < _TopMiddlePosition) {
                o.Albedo = lerp(_TopColor, _MiddleColor, gradientPosition / _TopMiddlePosition);
            } else {
                o.Albedo = lerp(_MiddleColor, _BottomColor, (gradientPosition - _TopMiddlePosition) / _MiddleBottomPosition);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
