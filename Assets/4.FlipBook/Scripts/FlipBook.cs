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

        var time = _timeKeeper.RecorderTime;

        // Frame sampling loop
        for (var (i, task) = (time.PageIndex - 1, (Awaitable)null);;)
        {
            await time.WaitPageAsync(++i);

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
        var time = _timeKeeper.PlayerTime;

        // Sequence progress parameters;
        var t_bg = time.SequenceProgress;
        var t_fg = time.SequenceProgressEased;

        // Page texture pairs
        var qidx = time.SequenceIndex % 2;
        var tex_bg = (GetPage(qidx, (int)t_bg - 1), GetPage(qidx, (int)t_bg));
        var tex_fg = (GetPage(qidx, (int)t_fg - 1), GetPage(qidx, (int)t_fg));

        // Derivative of the progress parameter (for motion blur)
        var ddt = time.SequenceProgressEasedDerivative;

        // Page rendering
        RenderBackPage (tex_bg, math.frac(t_bg), 1);
        RenderFrontPage(tex_fg, math.frac(t_fg), ddt);
    }
}

} // namespace Dcam2
