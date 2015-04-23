// This is the first step of the simulation process.
// We use RGB values as three-dimensional indices into a lookup table (which is actually a 2D texture),
// and retrieve a new color value which we write as the output of the pixel shader.

uniform float Tuning_Strength;			// typically [0,1], defines the strength of the color grading effect.

uniform texture cleanFrameTexture;		// This is the unaltered game scene, drawn to a render target texture at 256x224.
uniform texture lutTexture;				// This is a 32x32x32 lookup table, represented as a 1024x32 texture map.

uniform float GradingRes;				// This is the resolution of each dimension of the lookup table. Assumed to always be 32.

uniform sampler2D cleanFrameSampler = sampler_state
{
	Texture = (cleanFrameTexture);
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
	//AddressU = CLAMP;
	//AddressV = CLAMP;
};

uniform sampler2D lutSampler = sampler_state
{
	Texture = (lutTexture);
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
	//AddressU = CLAMP;
	//AddressV = CLAMP;
};

struct vsNTSCOut
{
	half4 pos		: POSITION;
	half2 uv		: TEXCOORD0;
};



// Passthrough vertex shader. Nothing interesting here.
vsNTSCOut NTSCVertexShader(	half3 pos : POSITION,
										half2 uv : TEXCOORD0)
{
	vsNTSCOut output;
	output.pos = half4(pos, 1);
	
	output.uv = uv;
	
	return output;
}



// Takes in a color value and returns a transformed color value from the lookup table.
half4 DoPost
(
	half4 InColor
)
{
	half4 GameScene = InColor;
		
	// Some of this stuff could be precomputed offline for perf.
	half Res = GradingRes;
	half RcpRes = 1.0f / Res;
	half ResSq = Res*Res;
	half RcpResSq = 1.0f / ResSq;
	half HalfRcpResSq = 0.5f * RcpResSq;
	half HalfRcpRes = 0.5f * RcpRes;
	
	half ResMinusOne = Res - 1.0f;
	half ResMinusOneOverRes = ResMinusOne / Res;
	half ResMinusOneOverResSq = ResMinusOne / ResSq;
	
	half2 graduv_lo;
	half2 graduv_hi;
	
	half b_lo = floor(InColor.b * ResMinusOne);
	half b_hi = ceil(InColor.b * ResMinusOne);
	half b_alpha = ((InColor.b * ResMinusOne) - b_lo);
	
	graduv_lo.x = (b_lo * RcpRes) + InColor.r * ResMinusOneOverResSq + HalfRcpResSq;
	graduv_lo.y = InColor.g * ResMinusOneOverRes + HalfRcpRes;
	
	graduv_hi.x = (b_hi * RcpRes) + InColor.r * ResMinusOneOverResSq + HalfRcpResSq;
	graduv_hi.y = InColor.g * ResMinusOneOverRes + HalfRcpRes;
	
	half4 postgrad_lo = tex2D(lutSampler, graduv_lo);
	half4 postgrad_hi = tex2D(lutSampler, graduv_hi);
	
	half4 postgrad = lerp(postgrad_lo, postgrad_hi, b_alpha);
		
	return lerp(InColor, postgrad, Tuning_Strength);
}



half4 NTSCPixelShader(vsNTSCOut frag) : COLOR
{
	return DoPost(tex2D(cleanFrameSampler, frag.uv));
}

technique NTSC
{
	pass Pass1
	{
		AlphaBlendEnable = false;
		//AlphaTestEnable = false;
		FillMode = solid;
		CullMode = cw;
		ZFunc = Always;
		ZWriteEnable = false;
		VertexShader = compile vs_2_0 NTSCVertexShader();
		PixelShader = compile ps_2_0 NTSCPixelShader();
	}
}
