using UnityEngine;
using Unity.Properties;
using Unity.Mathematics;

namespace Dcam2 {

public sealed partial class FlipBook
{
    #region Public properties (serialized)

    [field:SerializeField] public string Prompt { get; set; } = "painting";
    [field:SerializeField] public float Strength { get; set; } = 0.5f;
    [field:SerializeField] public float Guidance { get; set; } = 1.25f;

    #endregion

    #region Editable attributes

    [SerializeField] float _sampleInterval = 0.05f;
    [SerializeField] float _sequenceDuration = 1.2f;
    [SerializeField] float _easeOutPower = 4;
    [SerializeField] float _motionBlur = 0.1f;

    [SerializeField] string _resourceDir = "StableDiffusion";
    [SerializeField] Texture _source = null;

    #endregion

    #region Non-editable attributes

    [SerializeField, HideInInspector] Mesh _pageMesh = null;
    [SerializeField, HideInInspector] Material _pageMaterial = null;
    [SerializeField, HideInInspector] ComputeShader _sdPreprocess = null;

    #endregion

    #region Derived properties

    int QueueLength
      => (int)(_sequenceDuration / _sampleInterval);

    double SequenceDuration
      => QueueLength * (double)_sampleInterval;

    double LastPageDuration
      => SequenceDuration *
         math.pow(_sampleInterval / SequenceDuration, 1.0 / _easeOutPower);

    #endregion

    #if UNITY_EDITOR

    #region Editor UI 

    [CreateProperty]
    public string HintText
      => $"Last Page Duration: {LastPageDuration:0.000}";

    #endregion

    #endif
}

} // namespace Dcam2
