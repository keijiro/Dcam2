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
        // Time parameter normalized by the sequence length
        var t = CurrentPlayTime / SequenceDuration;

        // Ease-out applied time parameter (normalized)
        var t_p = 1 - math.pow(1 - math.frac(t), _easeOutPower);

        // Derivative of t_p, representing the flipping speed
        var dt_p = _easeOutPower * math.pow(1 - math.frac(t), _easeOutPower - 1);

        // Queue/page index
        var qidx = (int)t % 2;
        var pidx = (int)(t_p * QueueLength);

        // Page draw
        RenderPages(GetPage(qidx, pidx - 1),
                    GetPage(qidx, pidx),
                    math.frac((float)t_p * QueueLength),
                    _motionBlur * (float)dt_p);
    }
}

} // namespace Dcam2
