using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Dcam2 {

public sealed partial class FlipBook : MonoBehaviour
{
    Queue<RenderTexture> _backQueue;
    Queue<RenderTexture> _playQueue;
    float _playTime;

    async Awaitable Start()
    {
        // Initialization
        _backQueue = new Queue<RenderTexture>();
        _playQueue = new Queue<RenderTexture>();

        for (var i = 0; i < QueueLength; i++)
        {
            _backQueue.Enqueue(new RenderTexture(ImageWidth, ImageHeight, 0));
            _playQueue.Enqueue(new RenderTexture(ImageWidth, ImageHeight, 0));
        }

        InitializeRenderer();

        await InitializeGeneratorAsync();

        while (true)
        {
            for (var i = 0; i < QueueLength; i++)
            {
                var page = _backQueue.Dequeue();
                Graphics.Blit(_source, page);
                _backQueue.Enqueue(page);
                await Awaitable.WaitForSecondsAsync(PageInterval);
            }

            (_backQueue, _playQueue) = (_playQueue, _backQueue);
            _playTime = 0;
        }
    }

    void OnDestroy()
    {
        FinalizeGenerator();
        FinalizeRenderer();
        while (_backQueue.Count > 0) Destroy(_backQueue.Dequeue());
        while (_playQueue.Count > 0) Destroy(_playQueue.Dequeue());
    }

    void Update()
    {
        _playTime += Time.deltaTime / GenerationLatency;

        var pageTime = (1 - Mathf.Pow(1 - _playTime, 4)) * QueueLength;
        var index = Mathf.Min((int)pageTime, QueueLength - 2);

        RenderPages(_playQueue.ElementAt(index),
                    _playQueue.ElementAt(index + 1),
                    pageTime % 1);
    }
}

} // namespace Dcam2
