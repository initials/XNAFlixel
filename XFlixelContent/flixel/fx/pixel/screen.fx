// The final step of the CRT simulation process is to take the output of composite.fx (a 256x224 texture map)
// and draw it to a 3D mesh of a curved CRT screen. This is the step where we apply effects "outside the screen,"
// including the shadow mask, lighting, and so on.

uniform float2 UVScalar;
uniform float2 UVOffset;

uniform float2 CRTMask_Scale;
uniform float2 CRTMask_Offset;

uniform float Tuning_Overscan;
uniform float Tuning_Dimming;
uniform float Tuning_Satur;
uniform float Tuning_ReflScalar;
uniform float Tuning_Barrel;
uniform float Tuning_Scanline_Brightness;
uniform float Tuning_Scanline_Opacity;
uniform float Tuning_Diff_Brightness;
uniform float Tuning_Spec_Brightness;
uniform float Tuning_Spec_Power;
uniform float Tuning_Fres_Brightness;
uniform float3 Tuning_LightPos;

uniform texture compFrameMap;
uniform texture scanlinesMap;

uniform sampler2D compFrameSampler = sampler_state
{
	Texture = (compFrameMap);
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = BORDER;
	AddressV = BORDER;
	BorderColor = 0xff000000;
};

uniform sampler2D scanlinesSampler = sampler_state
{
	Texture = (scanlinesMap);
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = WRAP;
	AddressV = WRAP;
};

// Note: This "scope" markup is used by my engine and can be ignored.
uniform float4x4 wvpMat <string scope = "renderable";>;
uniform float4x4 worldMat <string scope = "renderable";>;

uniform float3 camPos;



struct vsScreenOut
{
	half4 pos		: POSITION;
	half4 color		: COLOR;
	half2 uv		: TEXCOORD0;
	half3 norm		: TEXCOORD1;
	half3 camDir	: TEXCOORD2;
	half3 lightDir	: TEXCOORD3;
};



// This vertex shader outputs direction vectors to the camera position, light position, etc.
// in additional texture coordinate channels. These are used by the pixel shader for doing per-pixel lighting.
vsScreenOut screenVertexShader(		half3 pos : POSITION,
									half3 norm : NORMAL,
									half4 color	: COLOR,
									half2 uv : TEXCOORD0)
{
	vsScreenOut output;
	output.pos = mul(half4(pos, 1), wvpMat);
	
	half3 worldPos = mul(half4(pos, 1), worldMat).xyz;
	
	output.color = color;
	output.uv = uv;
	
	output.norm = norm;
	
	half3x3 wMat3 = half3x3(worldMat[0].xyz,worldMat[1].xyz,worldMat[2].xyz);
	half3x3 invWorldRot = transpose(wMat3);
	
	// Don't normalize this pre-pixel shader
	output.camDir = mul(camPos - worldPos, invWorldRot);
	output.lightDir = mul(Tuning_LightPos - worldPos, invWorldRot);
	
	return output;
}

// Here we sample into the output of the compositing shader with some texture coordinate biz to simulate overscan and barrel distortion.
// We also apply the shadow mask (sometimes called "scanlines" here due to legacy naming) and saturation scaling.
half4 SampleCRT(half2 uv)
{
	half2 ScaledUV = uv;
	ScaledUV *= UVScalar;
	ScaledUV += UVOffset;
	
	half2 scanuv = ScaledUV * CRTMask_Scale;
	half3 scantex = tex2D(scanlinesSampler, scanuv).rgb;
	scantex += Tuning_Scanline_Brightness;		// Brighten up the shadow mask to mitigate darkening due to multiplication.
	scantex = lerp(half3(1,1,1), scantex, Tuning_Scanline_Opacity);

	// Apply overscan after scanline sampling is done.
	half2 overscanuv = (ScaledUV * Tuning_Overscan) - ((Tuning_Overscan - 1.0f) * 0.5f);
	
	// Curve UVs for composite texture inwards to garble things a bit.
	overscanuv = overscanuv - half2(0.5,0.5);
	half rsq = (overscanuv.x*overscanuv.x) + (overscanuv.y*overscanuv.y);
	overscanuv = overscanuv + (overscanuv * (Tuning_Barrel * rsq)) + half2(0.5,0.5);
		
	half3 comptex = tex2D(compFrameSampler, overscanuv).rgb;

	half4 emissive = half4(comptex * scantex, 1);
	half desat = dot(half4(0.299, 0.587, 0.114, 0.0), emissive);
	emissive = lerp(half4(desat,desat,desat,1), emissive, Tuning_Satur);
	
	return emissive;
}

// Here we sample the output of the compositing shader and apply Blinn-Phong lighting (diffuse + specular) plus a Fresnel rim lighting term.
half4 screenPixelShader(vsScreenOut frag) : COLOR
{	
	half3 norm = normalize(frag.norm);
	
	half3 camDir = normalize(frag.camDir);
	half3 lightDir = normalize(frag.lightDir);
	
	half3 refl = reflect(camDir, frag.norm);
		
	half diffuse = saturate(dot(norm, lightDir));
	half4 colordiff = half4(0.175, 0.15, 0.2, 1) * diffuse * Tuning_Diff_Brightness;
	
	half3 halfVec = normalize(lightDir + camDir);
	half spec = saturate(dot(norm, halfVec));
	spec = pow(spec, Tuning_Spec_Power);
	half4 colorspec = half4(0.25, 0.25, 0.25, 1) * spec * Tuning_Spec_Brightness;
	
	half fres = 1.0f - dot(camDir, norm);
	fres = (fres*fres) * Tuning_Fres_Brightness;
	half4 colorfres = half4(0.45, 0.4, 0.5, 1) * fres;
	
	half4 emissive = SampleCRT(frag.uv);
	
	half4 nearfinal = colorfres + colordiff + colorspec + emissive;
	
	// frag.color dims the edges of the CRT, but I don't really want that. CRTs don't do that!
	// This is really more about emulating ambient occlusion, but I'm applying it to the emissive, too.
	return (nearfinal * lerp(half4(1,1,1,1), frag.color, Tuning_Dimming));
}

technique Screen
{
	pass Pass1
	{
		AlphaBlendEnable = false;
		AlphaTestEnable = false;
		FillMode = solid;
		CullMode = cw;
		ZFunc = Less;
		ZWriteEnable = true;
		VertexShader = compile vs_2_0 screenVertexShader();
		PixelShader = compile ps_2_0 screenPixelShader();
	}
}
