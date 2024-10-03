using UnityEngine;
using Klak.TestTools;

namespace Dcam2 {

public sealed class PosterizerController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField, Range(0, 1)] public float Dithering { get; set; } = 0.5f;
    [field:SerializeField, Range(0, 1)] public float BackHue { get; set; }
    [field:SerializeField, Range(0, 1)] public float FrontHue { get; set; }

    public Material Material => UpdateMaterial();

    #endregion

    [SerializeField, HideInInspector] Shader _shader = null;

    Material _material;

    Material UpdateMaterial()
    {
        if (_material == null) _material = new Material(_shader);

        var h1 = BackHue;
        var h2 = (h1 + 0.333f) % 1;

        var h3 = FrontHue;
        var h4 = (h3 + 0.333f) % 1;

        var bg1 = Color.black;
        var bg2 = Color.HSVToRGB(h1, 1, 0.5f);
        var bg3 = Color.HSVToRGB(h2, 1, 0.8f);

        var fg1 = Color.HSVToRGB(h3, 1, 0.3f);
        var fg2 = Color.HSVToRGB(h4, 1, 1.0f);
        var fg3 = Color.white;

        var mbg = new Matrix4x4();
        var mfg = new Matrix4x4();

        mbg.SetRow(0, bg1);
        mbg.SetRow(1, bg1);
        mbg.SetRow(2, bg2);
        mbg.SetRow(3, bg3);

        mfg.SetRow(0, fg1);
        mfg.SetRow(1, fg1);
        mfg.SetRow(2, fg2);
        mfg.SetRow(3, fg3);

        _material.SetMatrix(ShaderID.BackPalette, mbg);
        _material.SetMatrix(ShaderID.FrontPalette, mfg);
        _material.SetFloat(ShaderID.Dither, Dithering);

        if (BackHue > 0)
            _material.EnableKeyword("_ENABLE_BACK");
        else
            _material.DisableKeyword("_ENABLE_BACK");

        if (FrontHue > 0)
            _material.EnableKeyword("_ENABLE_FRONT");
        else
            _material.DisableKeyword("_ENABLE_FRONT");

        return _material;
    }
}

} // namespace Dcam2
