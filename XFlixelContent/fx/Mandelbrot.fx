float2 Pan;
float Zoom;
float Aspect;
int Iterations;

float4 PixelShaderFunction(float4 texCoord : TEXCOORD0) : COLOR0
{
	float2 c = (texCoord - 0.5) * Zoom * float2(1, Aspect) - Pan;
	float2 v = 0;
	float4 result;
    int it = 0;
	float minV = 4;
	float minVx = 4;
	float minVy = 4;
		
	result.r = 0; result.g = 0;	result.b = 0; result.a = 0;

	while((it < Iterations) && (dot(v, v) < 8))
	{
		v = float2(v.x * v.x - v.y * v.y, v.x * v.y * 2) + c;

		if(length(v) < minV) { minV = length(v); }
		if(abs(v.x) < minVx) { minVx = abs(v.x); }
		if(abs(v.y) < minVy) { minVy = abs(v.y); }
		
		it = it +1;
	}
	
	if(it != Iterations)
	{
		//result =  (float)((float)it/(float)Iterations);
		//result = minV;
		result.r = (minVx);
		result.g = (minVy);
		result.b = minV;
	} else {
		result.r = (minVx);
		result.g = (minVy);
		result.b = minV;
	}
    
	return result;
}

technique
{
    pass
    {
	    PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}