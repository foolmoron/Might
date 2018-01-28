Shader "FX/Scramble Glitch"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_Width("Width", Range(0.0, 1.0)) = 0.1
		_Strength("Strength", Range(0.0, 1.0)) = 1
		_Freq("Frequency", Range(0.0, 100.0)) = 35
		_Step("Step", Range(0.0, 100)) = 15
		_EdgeSmoothing("Edge Smoothing", Range(0.0, 10.0)) = 4
		_Timescale("Timescale", Range(0.0, 40)) = 1
		_CenterX("Center", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
			
			float _Width;
			float _Strength;
			float _Freq;
			float _Step;
			float _EdgeSmoothing;
			float _Timescale;
			float _CenterX;

			float rand(float n){return frac(sin(n) * 43758.5453123);}

			fixed4 frag(v2f IN) : SV_Target
			{
				float2 uv = IN.texcoord;
				float min = _CenterX - (_Width / 2);
				float max = _CenterX + (_Width / 2);
				float wave = (sin(uv.y * _Freq + _Time.w * _Timescale) + 1) / 2;
				wave = floor(wave * _Step) / _Step;
				float dist = (uv.x - min) / _Width;
				float stretch = 0;
				if (dist < wave) {
					stretch = pow(dist / wave, 2);
				} else {
					stretch = -pow((1 - dist) / (1 - wave), 2);
				}
				stretch *= 1 - pow(abs(dist - 0.5) * 2, _EdgeSmoothing);
				stretch *= _Strength;
				if (uv.x >= min && uv.x < max) {
					if (dist < wave) {
						uv.x = min + stretch * _Width / 2;
					} else {
						uv.x = max - stretch * _Width / 2;
					}
				}
				fixed4 c = SampleSpriteTexture(uv) * IN.color;
				c.rgb *= c.a;
				return c;
			}
        ENDCG
        }
    }
}
