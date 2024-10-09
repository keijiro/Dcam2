using UnityEngine;
using Unity.Mathematics;

namespace Dcam2 {

// FlipBook - Main MonoBehaviour implementation

public sealed partial class FlipBook : MonoBehaviour
{
    double _time;

    async Awaitable Start()
    {
        InitializeQueue();
        InitializeRenderer();
        await InitializeGeneratorAsync();

        for (var last = -1;;)
        {
            var idx = (int)(_time / _sampleInterval);
            if (idx > last)
            {
                var qidx = idx / QueueLength % 2;
                var pidx = idx % QueueLength;
                var page = GetPage(qidx, pidx);
                Graphics.Blit(_source, page);
                last = idx;
            }
            await Awaitable.NextFrameAsync();
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
        if (_time >= LastPageDuration + SequenceDuration) DrawPages();
        _time += Time.deltaTime;
    }

    void DrawPages()
    {
        var t = (_time - LastPageDuration) / SequenceDuration - 1;
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
