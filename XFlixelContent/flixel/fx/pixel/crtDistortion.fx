sampler ColorMapSampler : register(s0); 
 
// Apply radial distortion to the given coordinate. 
float2 radialDistortion(float2 coord, float2 pos) 
{ 
    float distortion = 1.205; 
 
    float2 cc = pos - 0.5; 
    float dist = dot(cc, cc) * distortion; 
    return coord * (pos + cc * (1.0 + dist) * dist) / pos; 
} 
 
float4 PixelShaderFunction(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0 
{ 
    texCoord = radialDistortion(texCoord, texCoord); 
    return tex2D(ColorMapSampler, texCoord);  
} 
 
technique Bend 
{ 
    pass Pass1 
    { 
        PixelShader = compile ps_2_0 PixelShaderFunction(); 
    } 
} 