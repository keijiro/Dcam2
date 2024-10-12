using UnityEngine;

namespace Dcam2 {

// FlipBook - Properties

public sealed partial class FlipBook
{
    #region Public properties (serialized)

    [field:SerializeField] public string Prompt { get; set; } = "painting";
    [field:SerializeField] public float Strength { get; set; } = 0.5f;
    [field:SerializeField] public float Guidance { get; set; } = 1.25f;
    [field:SerializeField] public float MotionBlur { get; set; } = 0.2f;

    #endregion

    #region Editable attributes

    [SerializeField] TimeKeeper _time = null;
    [SerializeField] string _generatorResourceDir = "StableDiffusion";
    [SerializeField] bool _loadGeneratorResource = true;
    [SerializeField] Texture _source = null;

    #endregion

    #region Non-editable attributes

    [SerializeField, HideInInspector] Mesh _pageMesh = null;
    [SerializeField, HideInInspector] Material _pageMaterial = null;
    [SerializeField, HideInInspector] ComputeShader _sdPreprocess = null;

    #endregion
}

} // namespace Dcam2
