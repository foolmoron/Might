Shader "Sprites/Offset"
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
		[HideInInspector] _T("T", Float) = 0
        [PerRendererData] _OffsetX ("OffsetX", Float) = 0
        [PerRendererData] _OffsetY ("OffsetY", Float) = 0
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
			
			float _OffsetX;
			float _OffsetY;

			fixed4 frag(v2f IN) : SV_Target
			{
				float2 uv = IN.texcoord;
				uv = fmod(uv + float2(_OffsetX, _OffsetY), 1);
				fixed4 c = SampleSpriteTexture(uv) * IN.color;
				c.rgb *= c.a;
				return c;
			}
        ENDCG
        }
    }
}
