using UnityEngine;
using ImageSource = Klak.TestTools.ImageSource;

namespace Dcam2 {

public sealed partial class Flipbook
{
    #region Public Properties (Serialized)

    [field:Header("Stable Diffusion")]
    [field:SerializeField] public string Prompt { get; set; } = "painting";
    [field:SerializeField] public float Strength { get; set; } = 0.5f;
    [field:SerializeField] public float Guidance { get; set; } = 1.25f;

    [field:Header("Flip animation")]
    [field:SerializeField] public float FlipDuration { get; set; } = 0.175f;
    [field:SerializeField] public int QueueLength { get; set; } = 9;
    [field:SerializeField] public int InsertionCount { get; set; } = 5;

    #endregion

    #region Editable attributes

    [Header("References")]
    [SerializeField] string _resourceDir = "StableDiffusion";
    [SerializeField] Texture _source = null;

    #endregion

    #region Non-editable attributes

    [SerializeField, HideInInspector] Mesh _pageMesh = null;
    [SerializeField, HideInInspector] Material _pageMaterial = null;
    [SerializeField, HideInInspector] ComputeShader _sdPreprocess = null;

    #endregion
}

} // namespace Dcam2
