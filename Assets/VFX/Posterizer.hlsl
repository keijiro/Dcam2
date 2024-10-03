//
// Posterize effect
//

// Bayer matrix for dithering
static const float ImagePlane_BayerArray[] =
{
    0.000000, 0.871094, 0.371094,
    0.746094, 0.621094, 0.246094,
    0.496094, 0.121094, 0.996094,
};

// Dithering function
static float ImagePlane_Dither(uint2 psp)
{
    return ImagePlane_BayerArray[(psp.x % 3) + (psp.y % 3) * 3];
}

void ImagePlanePosterize_float
    (float4 Input,
     uint2 PixelPos,
     float4x4 Palette,
     float Dither,
     out float3 Output)
{
    float dither = ImagePlane_Dither(PixelPos) * Dither;
    float lum = saturate(Luminance(LinearToSRGB(Input.rgb) + dither));
    Output = Palette[(uint)(lum * 3.9999)].rgb;
}
