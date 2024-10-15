using UnityEngine;
using ComputeUnits = MLStableDiffusion.ComputeUnits;
using SDPipeline = MLStableDiffusion.Pipeline;
using Scheduler = MLStableDiffusion.Scheduler;

namespace Dcam2 {

public sealed class ImageGenerator : MonoBehaviour
{
    #region Public constants

    public const int Width = 640;
    public const int Height = 384;
    public const float AspectRatio = (float)Width / Height;

    #endregion

    #region Public properties

    [field:SerializeField] public string Prompt { get; set; } = "painting";
    [field:SerializeField] public float Strength { get; set; } = 0.5f;
    [field:SerializeField] public int StepCount { get; set; } = 4;
    [field:SerializeField] public float Guidance { get; set; } = 1.25f;

    #endregion

    #region Editable attributes

    [SerializeField] bool _loadResource = true;

    #endregion

    #region Project asset reference

    [SerializeField, HideInInspector] ComputeShader _preprocess = null;

    #endregion

    #region Private memeber

    SDPipeline _pipeline;

    static string ResourcePath
      => Application.streamingAssetsPath + "/StableDiffusion";

    #endregion

    #region Public Method

    public async Awaitable RunGeneratorAsync(RenderTexture src,
                                             RenderTexture dest)
    {
        if (_pipeline != null)
        {
            _pipeline.Prompt = Prompt;
            _pipeline.Strength = Strength;
            _pipeline.StepCount = StepCount;
            _pipeline.GuidanceScale = Guidance;
            _pipeline.Seed = Random.Range(1, 2000000000);
            await _pipeline.RunAsync(src, dest, destroyCancellationToken);
        }
        else
        {
            if (src != dest) Graphics.Blit(src, dest);
            await Awaitable.WaitForSecondsAsync(1);
        }
    }

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        if (!_loadResource) return;

        _pipeline = new SDPipeline(_preprocess);
        _pipeline.Scheduler = Scheduler.Lcm;

        Debug.Log("Loading the Stable Diffusion model...");

        var resource = MLStableDiffusion.ResourceInfo.
          FixedSizeModel(ResourcePath, Width, Height);

        await _pipeline.InitializeAsync(resource, ComputeUnits.CpuAndGpu);

        Debug.Log("Done.");
    }

    void OnDestroy()
    {
        _pipeline?.Dispose();
        _pipeline = null;
    }

    #endregion
}


} // namespace Dcam2
