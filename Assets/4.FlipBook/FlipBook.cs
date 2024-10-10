using UnityEngine;
using Unity.Mathematics;

namespace Dcam2 {

// FlipBook - Main MonoBehaviour implementation

public sealed partial class FlipBook : MonoBehaviour
{
    async Awaitable Start()
    {
        InitializeQueue();
        InitializeRenderer();
        await InitializeGeneratorAsync();

        for (var (last, genTask) = (-1, (Awaitable)null);;)
        {
            var idx = (int)(Time.timeAsDouble / _sampleInterval);

            if (idx == last)
            {
                await Awaitable.NextFrameAsync();
                continue;
            }

            var qidx = idx / QueueLength % 2;
            var pidx = idx % QueueLength;
            var page = GetPage(qidx, pidx);

            Graphics.Blit(_source, page);

            if (pidx == QueueLength - 1)
            {
                if (genTask != null && !genTask.IsCompleted)
                    Debug.Log("Generator task not completed");
                else
                    genTask = RunGeneratorAsync(page, page);
            }

            last = idx;
        }
    }

    void OnDestroy()
    {
        FinalizeGenerator();
        FinalizeRenderer();
        FinalizeQueue();
    }

    void Update()
      => DrawPages();

    void DrawPages()
    {
        var t = (Time.timeAsDouble - LastPageDuration) / SequenceDuration - 1;
        var t_p = 1 - math.pow(1 - math.frac(t), _easeOutPower);
        var dt_p = _easeOutPower * math.pow(1 - math.frac(t), _easeOutPower - 1);

        var qidx = (int)t % 2;
        var pidx = (int)(t_p * QueueLength);

        RenderPages(GetPage(qidx, pidx - 1),
                    GetPage(qidx, pidx),
                    math.frac((float)t_p * QueueLength),
                    _motionBlur * (float)dt_p);
    }
}

} // namespace Dcam2
