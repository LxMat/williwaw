Shader "Custom/GradientWaves"
{
	Properties
	{
		_LightColor("Water Color Highlight", Color) = (1,1,1,1)
		_DarkColor("Water Color Shadow",Color) = (1,1,1,1)
		_GradientRange("Gradient Range",Range(0,5)) = 2.5
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Wave1("Wave1 (2D-dir,steepness,wavelength)",Vector) = (1,0,0.6,30)
		_Wave2("Wave2",Vector) = (1,1,0.3,50)
		_Wave3("Wave3",Vector) = (1,-1,0.4,20)
		_Wave4("Wave4",Vector) = (0.5,0.7,0.6,70)
		_Wave5("Wave5",Vector) = (0.5,0.7,0.6,70)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows vertex:vert 

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;

	struct Input
	{
		float2 uv_MainTex;
		float3 WorldPos;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _LightColor,_DarkColor;
	float _GradientRange;

	float4 _Wave1, _Wave2, _Wave3, _Wave4,_Wave5;
	
	//float4 _Waves[5] = { _Wave1, _Wave2, _Wave3, _Wave4, _Wave5 };

	UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)


		//most of the code is from catlike coding.
		float3 GerstnerWave(
			float4 wave, float3 p, inout float3 tangent, inout float3 binormal
		) {
		float steepness = wave.z;
		float wavelength = wave.w;
		float k = 2 * UNITY_PI / wavelength;
		float c = sqrt(9.8 / k);
		float2 d = normalize(wave.xy);
		float f = k * (dot(d, p.xz) - c * _Time.y);
		float a = steepness / k;

		tangent += float3(
			-d.x * d.x * (steepness * sin(f)),
			d.x * (steepness * cos(f)),
			(1 - d.x * d.y * (steepness * sin(f)))
			);
		binormal += float3(
			(1 - d.x * d.y * (steepness * sin(f))),
			d.y * (steepness * cos(f)),
			-d.y * d.y * (steepness * sin(f))
			);
		return float3(
			d.x * (a * cos(f)),
			a * sin(f),
			d.y * (a * cos(f))
			);

	}

	void vert(inout appdata_full vertexdata, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);

		float3 gridPoint = vertexdata.vertex.xyz;
		float3 tangent = float3(0, 0, 1);
		float3 binormal = float3(1, 0, 0);
		float3 p = gridPoint;
		p += GerstnerWave(_Wave1, gridPoint, tangent, binormal);
		p += GerstnerWave(_Wave2, gridPoint, tangent, binormal);
		p += GerstnerWave(_Wave3, gridPoint, tangent, binormal);
		p += GerstnerWave(_Wave4, gridPoint, tangent, binormal);
		p += GerstnerWave(_Wave5, gridPoint, tangent, binormal);

		vertexdata.vertex.y = p.y;
		tangent = normalize(tangent);
		binormal = normalize(binormal);
		float3 normal = normalize(cross(tangent, binormal));
		vertexdata.normal = normal;
		o.WorldPos = p;
	}


	void surf(Input IN, inout SurfaceOutputStandard o)
	{


		fixed4 test = lerp(_DarkColor, _LightColor, IN.WorldPos.y * _GradientRange);

		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * test;
		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	ENDCG
	}
		//    FallBack "Diffuse"
}
