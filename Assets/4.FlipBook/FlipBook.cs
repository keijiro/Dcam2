using UnityEngine;
using Unity.Mathematics;

namespace Dcam2 {

// FlipBook - Main MonoBehaviour implementation

public sealed partial class FlipBook : MonoBehaviour
{
    async Awaitable Start()
    {
        // Initialization
        InitializeQueue();
        InitializeRenderer();
        await InitializeGeneratorAsync();

        // Frame sampling loop
        for (var (i, task) = (CurrentSampleIndex - 1, (Awaitable)null);;)
        {
            await WaitSampleIndex(++i);

            // Queue/page index
            var qidx = i / QueueLength % 2;
            var pidx = i % QueueLength;

            // Sample
            var page = GetPage(qidx, pidx);
            Graphics.Blit(_source, page);

            // Last page: Generator invocation
            if (pidx == QueueLength - 1)
            {
                if (task?.IsCompleted ?? true)
                    task = RunGeneratorAsync(page, page);
                else
                    Debug.Log("Previous generator task not completed");
            }
        }
    }

    void OnDestroy()
    {
        FinalizeGenerator();
        FinalizeRenderer();
        FinalizeQueue();
    }

    void Update()
    {
        // Normalized time parameter in the sequence
        var t = math.frac(CurrentPlayTime / SequenceDuration);

        // Ease-out applied time parameter
        var t_p = 1 - math.pow(1 - t, _easeOutPower);

        // Derivative of t_p, representing the flipping speed
        var dt_p =
          _easeOutPower * math.pow(1 - t, _easeOutPower - 1);

        // Queue/page indices
        var qidx = (int)(CurrentPlayTime / SequenceDuration) % 2;
        var pidx_bg = (int)(QueueLength * t);
        var pidx_fg = (int)(QueueLength * t_p);

        // Page flipping progress parameters
        var prog_bg = math.frac(QueueLength * t);
        var prog_fg = math.frac(QueueLength * t_p);

        // Page textures
        var tex_bg = (GetPage(qidx, pidx_bg - 1), GetPage(qidx, pidx_bg));
        var tex_fg = (GetPage(qidx, pidx_fg - 1), GetPage(qidx, pidx_fg));

        // Page draw
        RenderBackPage(tex_bg, prog_bg, 1);
        RenderFrontPage(tex_fg, prog_fg, dt_p);
    }
}

} // namespace Dcam2
