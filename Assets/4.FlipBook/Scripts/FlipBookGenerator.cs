using UnityEngine;
using ComputeUnits = MLStableDiffusion.ComputeUnits;
using SDPipeline = MLStableDiffusion.Pipeline;
using Scheduler = MLStableDiffusion.Scheduler;

namespace Dcam2 {

// FlipBook - Generator (Stable Diffusion pipeline) implementation

public sealed partial class FlipBook
{
    const int ImageWidth = 640;
    const int ImageHeight = 384;

    string GeneratorResourcePath
      => Application.streamingAssetsPath + "/" + _generatorResourceDir;

    MLStableDiffusion.ResourceInfo GeneratorResourceInfo
      => MLStableDiffusion.ResourceInfo.FixedSizeModel
           (GeneratorResourcePath, ImageWidth, ImageHeight);

    SDPipeline _generator;

    async Awaitable InitializeGeneratorAsync()
    {
        if (!_loadGeneratorResource) return;

        _generator = new SDPipeline(_sdPreprocess)
          { Scheduler = Scheduler.Lcm, StepCount = 4 };

        Debug.Log("Loading the Stable Diffusion model...");

        await _generator.InitializeAsync
          (GeneratorResourceInfo, ComputeUnits.CpuAndGpu);

        Debug.Log("Done.");
    }

    void FinalizeGenerator()
    {
        _generator?.Dispose();
        _generator = null;
    }

    async Awaitable RunGeneratorAsync(RenderTexture src, RenderTexture dest)
    {
        if (_generator != null)
        {
            _generator.Prompt = Prompt;
            _generator.Strength = Strength;
            _generator.GuidanceScale = Guidance;
            _generator.Seed = Random.Range(1, 2000000000);
            await _generator.RunAsync(src, dest, destroyCancellationToken);
        }
        else
        {
            Graphics.Blit(src, dest);
            await Awaitable.WaitForSecondsAsync(1);
        }
    }
}

} // namespace Dcam2
