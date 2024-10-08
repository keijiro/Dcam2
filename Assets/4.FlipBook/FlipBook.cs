using UnityEngine;
using Unity.Mathematics;

namespace Dcam2 {

public sealed partial class FlipBook : MonoBehaviour
{
    RenderTexture[][] _queue;
    double _time;

    async Awaitable Start()
    {
        // Initialization
        _queue = new RenderTexture[2][];
        _queue[0] = new RenderTexture[QueueLength];
        _queue[1] = new RenderTexture[QueueLength];

        for (var i = 0; i < QueueLength; i++)
        {
            _queue[0][i] = new RenderTexture(ImageWidth, ImageHeight, 0);
            _queue[1][i] = new RenderTexture(ImageWidth, ImageHeight, 0);
        }

        InitializeRenderer();

        await InitializeGeneratorAsync();

        for (var last = -1;;)
        {
            var idx = (int)(_time / _sampleInterval);
            if (idx > last)
            {
                var qidx = idx / QueueLength % 2;
                var pidx = idx % QueueLength;
                var page = _queue[qidx][pidx];
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
        for (var i = 0; i < QueueLength; i++)
        {
            Destroy(_queue[0][i]);
            Destroy(_queue[1][i]);
        }
    }

    void Update()
    {
        if (_time >= LastPageDuration + SequenceDuration) DrawPages();
        _time += Time.deltaTime;
    }

    RenderTexture GetPage(int qidx, int pidx)
        => pidx >= 0 ? _queue[qidx][pidx] : _queue[qidx ^ 1][QueueLength - 1];

    void DrawPages()
    {
        var t = (_time - LastPageDuration) / SequenceDuration - 1;
        var qidx = (int)t % 2;
        var eased = 1 - math.pow(1 - math.frac(t), _easeOutPower);
        var pidx = (int)(eased * QueueLength);
        var prog = math.frac(eased * QueueLength);
        RenderPages(GetPage(qidx, pidx - 1), GetPage(qidx, pidx), (float)prog);
    }
}

} // namespace Dcam2
