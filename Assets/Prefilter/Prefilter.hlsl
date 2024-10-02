#include "Packages/jp.keijiro.bodypix/Shaders/Common.hlsl"

void BodyPixFilterMain_float
  (UnityTexture2D Video,
   UnityTexture2D BodyPix,
   UnityTexture3D LUT,
   float2 UV,
   out float4 Output)
{
    // Video input + LUT
    float3 srgb = LinearToSRGB(tex2D(Video, UV).rgb);
    float3 graded = SRGBToLinear(tex3D(LUT, srgb).rgb);

    // Human stencil
    BodyPix_Mask mask = BodyPix_SampleMask(UV, BodyPix.tex, BodyPix.texelSize.zw);
    float alpha = smoothstep(0.4, 0.6, BodyPix_EvalSegmentation(mask));

    Output = float4(graded, alpha);
}
