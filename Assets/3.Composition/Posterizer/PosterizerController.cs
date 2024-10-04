using UnityEngine;
using UnityEngine.Rendering;
using Klak.TestTools;

namespace Dcam2 {

public sealed class PosterizerController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField, Range(0, 1)] public float Dithering { get; set; }
    [field:SerializeField, Range(0, 1)] public float BackHue { get; set; }
    [field:SerializeField, Range(0, 1)] public float FrontHue { get; set; }

    public bool IsReady => _material != null;
    public Material Material => UpdateMaterial();

    #endregion

    #region Project asset reference

    [SerializeField, HideInInspector] Shader _shader = null;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _material = new Material(_shader);
        _enableBack = new LocalKeyword(_shader, "_ENABLE_BACK");
        _enableFront = new LocalKeyword(_shader, "_ENABLE_FRONT");
    }

    #endregion

    #region Private members

    Material _material;
    LocalKeyword _enableBack, _enableFront;

    Material UpdateMaterial()
    {
        // Color palette
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

        // Properties
        _material.SetMatrix(ShaderID.BackPalette, mbg);
        _material.SetMatrix(ShaderID.FrontPalette, mfg);
        _material.SetFloat(ShaderID.Dither, Dithering);

        // Keywords
        if (BackHue > 0)
            _material.EnableKeyword(_enableBack);
        else
            _material.DisableKeyword(_enableBack);

        if (FrontHue > 0)
            _material.EnableKeyword(_enableFront);
        else
            _material.DisableKeyword(_enableFront);

        return _material;
    }

    #endregion
}

} // namespace Dcam2
