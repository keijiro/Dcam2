using UnityEngine;

namespace Dcam2 {

static class ShaderID
{
    public static readonly int BackPalette = Shader.PropertyToID("_BackPalette");
    public static readonly int BodyPixTex = Shader.PropertyToID("_BodyPixTex");
    public static readonly int Dither = Shader.PropertyToID("_Dither");
    public static readonly int FrontPalette = Shader.PropertyToID("_FrontPalette");
    public static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public static readonly int Opacity = Shader.PropertyToID("_Opacity");
    public static readonly int LutTex = Shader.PropertyToID("_LutTex");
}

} // namespace Dcam2
