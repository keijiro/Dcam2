using UnityEngine;

namespace Dcam2 {

static class ShaderID
{
    public static readonly int BodyPixTex = Shader.PropertyToID("_BodyPixTex");
    public static readonly int Dither = Shader.PropertyToID("_Dither");
    public static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public static readonly int Opacity = Shader.PropertyToID("_Opacity");
    public static readonly int PaletteBG = Shader.PropertyToID("_PaletteBG");
    public static readonly int PaletteFG = Shader.PropertyToID("_PaletteFG");
    public static readonly int LutTex = Shader.PropertyToID("_LutTex");
}

} // namespace Dcam2
