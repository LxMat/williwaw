Shader "Unlit/WaterWaves"
{
	Properties
	{
		[Header(Water Appearance)]
	_DepthGradientShallow("Shallow Water Color", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthGradientDeep("Deep Water Color", Color) = (0.086, 0.407, 1, 0.749)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1
		_FoamColor("FoamColor",Color) = (0,0,0)
		//_SurfaceNoice("Surface Noise",2D) = "white" {}
		_WaterColor("Water Color",Color) = (1,1,1)



		[Header(Wave Properties)]
	_Speed("Wave Speed",Float) = 0.5
		_Steepness("Wave Steepness",Range(0,1)) = 0.1
		_Wavelength("Wavelength",Float) = 5
		_Direction("Wave Direction (2D)",Vector) = (1,0,0,0)
		//_Amplitude("Amplitude",Float) = 3
	}
		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		//Wave cosmetics, color and stufff
		float4 _DepthGradientShallow;
	float4 _DepthGradientDeep;
	float4 _WaterColor;
	float4 _FoamColor;
	float _DepthMaxDistance;

	sampler2D _CameraDepthTexture;
	sampler2D _NoiseTex;

	//Wave physics properties
	float _Speed,_Steepness,_Wavelength,_Amplitude;
	float2 _Direction;


	struct appdata
	{
		float4 vertex : POSITION;
		float4 color : COLOR;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float4 screenPosition : TEXCOORD2;
		float4 color : COLOR;

	};



	//wave offset mostly from catlikecoding...
	v2f vert(appdata_full v)
	{
		v2f o;
		half3 p = v.vertex;

		//Gerstner Wave offset
		float k = 2 * UNITY_PI / _Wavelength;
		float c = sqrt(9.8 / k);
		float2 d = normalize(_Direction);
		float f = k * (dot(d, p.xz) - c * _Time.y);
		float a = _Steepness / k;

		p.x += d.x * ((_Steepness / k) * cos(f));
		p.y += (_Steepness / k) * sin(f);
		p.z += d.y * ((_Steepness / k) * cos(f));

		v.vertex.xyz = p;


		o.color = clamp(((p.y + 1) / 2)*v.color, 0.3, 1);
		//o.color = _WaterColor;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.screenPosition = ComputeScreenPos(o.vertex);

		return o;
	}

	float4 frag(v2f i) : SV_Target
	{
		float _Foam = 5;
	float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition))); // depth
	float4 foamLine = 1 - saturate(_Foam* (depth - i.screenPosition.w));// foam line by comparing depth and screenposition
	float depthDifference2 = depth - i.screenPosition.w;
	float waterDepthDifference = saturate(depthDifference2 / _DepthMaxDistance); //Change how much depth we perceive
	float4 waterColor2 = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference); //Add a nice colored gradient

	fixed distort = tex2D(_NoiseTex, i.vertex.xz + (_Time.x * 2)).r;// distortion
	float3 waterColor3 = lerp(waterColor2.rgb*_DepthGradientDeep.rgb, waterColor2*_DepthGradientShallow.rgb, i.color.r);
	return waterColor2;
	}
		ENDCG
	}
	}
}
