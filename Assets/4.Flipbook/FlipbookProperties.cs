using UnityEngine;

namespace Dcam2 {

public sealed partial class FlipBook
{
    #region Public Properties (Serialized)

    [field:Header("Stable Diffusion")]
    [field:SerializeField] public string Prompt { get; set; } = "painting";
    [field:SerializeField] public float Strength { get; set; } = 0.5f;
    [field:SerializeField] public float Guidance { get; set; } = 1.25f;

    [field:Header("Flip animation")]
    [field:SerializeField] public float GenerationLatency { get; set; } = 1.2f;
    [field:SerializeField] public float PauseDuration { get; set; } = 0.3f;
    [field:SerializeField] public float PageInterval { get; set; } = 0.05f;

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

    #region Derived properties

    int QueueLength => Mathf.CeilToInt(GenerationLatency / PageInterval);

    #endregion
}

} // namespace Dcam2
