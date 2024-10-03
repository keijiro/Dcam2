using UnityEngine;
using Klak.TestTools;
using BodyPix;

namespace Dcam2 {

public sealed class Prefilter : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Texture3D _lutTexture = null;
    [SerializeField] RenderTexture _output = null;

    #endregion

    #region Project asset references

    [SerializeField, HideInInspector] Shader _shader = null;

    #endregion

    #region Public properties

    public GraphicsBuffer KeypointBuffer => _detector.KeypointBuffer;

    #endregion

    #region Private members

    BodyDetector _detector;
    Material _material;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _detector = new BodyDetector(_resources, 512, 384);
        _material = new Material(_shader);
    }

    void OnDestroy()
    {
        _detector.Dispose();
        Destroy(_material);
    }

    void LateUpdate()
    {
        _detector.ProcessImage(_source.AsTexture);
        _material.SetTexture(ShaderID.BodyPixTex, _detector.MaskTexture);
        _material.SetTexture(ShaderID.LutTex, _lutTexture);
        Graphics.Blit(_source.AsTexture, _output, _material);
    }

    #endregion
}

} // namespace Dcam2
