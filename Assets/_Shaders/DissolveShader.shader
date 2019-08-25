Shader "Custom/DissolveSurface" {
	Properties{
	_Color("MainColor", Color) = (1,1,1,1)
	_DissolveColor("DissolveColor", Color) = (1,1,1,1)
	_DissolveWidth("DissolveWidth", float) = 0.05
	_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
	_Metallic("Metallic", Range(0,1)) = 0.0

	//Dissolve properties
	_DissolveTexture("Dissolve Texutre", 2D) = "white" {}

	//For script with MaterialPropertyBlocks
	[PerRendererData]_Amount("Amount", Range(0,1)) = 0
	//_Amount("Amount", Range(0,1)) = 0
	}

		SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Cull Off //Fast way to turn your material double-sided

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		//Important: Enable GPU Instancing
		#pragma multi_compile_instancing

		sampler2D _MainTex;

		struct Input {
		float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		
		//Dissolve properties
		sampler2D _DissolveTexture;
		half _Amount;
		fixed4 _DissolveColor;
		float _DissolveWidth;

		void surf(Input IN, inout SurfaceOutputStandard o) {

			//Dissolve function
			half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
			clip(dissolve_value - _Amount);

			//Dissolve border emits white color with 0.05 border size
			o.Emission = _DissolveColor * step(dissolve_value - _Amount, _DissolveWidth);

			//Basic shader function
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}