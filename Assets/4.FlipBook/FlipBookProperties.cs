using UnityEngine;
using Unity.Properties;

namespace Dcam2 {

public sealed partial class FlipBook
{
    #region Public Properties (Serialized)

    [field:Header("Stable Diffusion")]
    [field:SerializeField] public string Prompt { get; set; } = "painting";
    [field:SerializeField] public float Strength { get; set; } = 0.5f;
    [field:SerializeField] public float Guidance { get; set; } = 1.25f;

    [field:Header("Flip animation")]
    [field:SerializeField] public float SampleInterval { get; set; } = 0.05f;
    [field:SerializeField] public float SequenceLength { get; set; } = 1.2f;
    [field:SerializeField] public float EaseOutPower { get; set; } = 4;

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

    int QueueLength => Mathf.CeilToInt(SequenceLength / SampleInterval);

    #endregion

    [CreateProperty]
    public float LastPageTime
        => SequenceLength *
           (1 - Mathf.Pow(SampleInterval / SequenceLength, 1.0f / EaseOutPower));
}

} // namespace Dcam2
