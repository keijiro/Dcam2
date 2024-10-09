using UnityEngine;

namespace Dcam2 {

static class ShaderID
{
    public static readonly int Aspect = Shader.PropertyToID("_Aspect");
    public static readonly int BackPalette = Shader.PropertyToID("_BackPalette");
    public static readonly int BaseTex = Shader.PropertyToID("_BaseTex");
    public static readonly int Blur = Shader.PropertyToID("_Blur");
    public static readonly int BodyPixTex = Shader.PropertyToID("_BodyPixTex");
    public static readonly int Dither = Shader.PropertyToID("_Dither");
    public static readonly int FlipTex = Shader.PropertyToID("_FlipTex");
    public static readonly int FrontPalette = Shader.PropertyToID("_FrontPalette");
    public static readonly int LutTex = Shader.PropertyToID("_LutTex");
    public static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public static readonly int Occlusion = Shader.PropertyToID("_Occlusion");
    public static readonly int Opacity = Shader.PropertyToID("_Opacity");
    public static readonly int Progress = Shader.PropertyToID("_Progress");
}

} // namespace Dcam2
