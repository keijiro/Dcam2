using UnityEngine;
using Klak.TestTools;

namespace Dcam2 {

public sealed class ImagePlaneController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] Texture Source { get; set; }
    [field:SerializeField, Range(0, 1)] float Dithering = 0.5f;
    [field:SerializeField] public float BackgroundOpacity { get; set; } = 1;
    [field:SerializeField] public float ForegroundOpacity { get; set; } = 1;

    #endregion

    #region Public methods

    public void ShufflePalette()
    {
        var h1 = Random.value;
        var h2 = (h1 + 0.333f) % 1;

        var h3 = Random.value;
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

        _prop.SetMatrix(ShaderID.PaletteBG, mbg);
        _prop.SetMatrix(ShaderID.PaletteFG, mfg);
    }

    #endregion

    MaterialPropertyBlock _prop;

    void Start()
    {
        _prop = new MaterialPropertyBlock();
        ShufflePalette();
    }

    void Update()
    {
        _prop.SetTexture(ShaderID.MainTex, Source);
        _prop.SetFloat(ShaderID.Dither, Dithering);
        _prop.SetVector(ShaderID.Opacity, new Vector2(BackgroundOpacity, ForegroundOpacity));
        GetComponent<Renderer>().SetPropertyBlock(_prop);
    }
}

} // namespace Dcam2
