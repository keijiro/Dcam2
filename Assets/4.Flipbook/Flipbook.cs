using UnityEngine;

namespace Dcam2 {

public sealed partial class Flipbook : MonoBehaviour
{
    async Awaitable Start()
    {
        // Initialization
        await InitObjects();

        for (var genTask = (Awaitable)null;;)
        {
            // Push the previous "latest" frame to the queue.
            _frameQueue.Enqueue(_latestFrame);

            // Reuse the previous "sheet" frame to store the latest frame.
            _latestFrame = _bgFrames.sheet;
            Graphics.Blit(_source, _latestFrame);

            // The previous "flip" frame becomes the "sheet" frame.
            _bgFrames.sheet = _bgFrames.flip;

            // Get a frame from the queue and make it flipping.
            _bgFrames.flip = _frameQueue.Dequeue();

            // Flip animation restart
            _flipTime = 0;

            // Generator task cycle
            if (_flipCount >= QueueLength && (genTask == null || genTask.IsCompleted))
            {
                _fgFrames = (_fgFrames.front, _fgFrames.back);
                genTask = RunSDPipelineAsync();
                _flipCount = 0;
            }

            // Per-flip wait
            await Awaitable.WaitForSecondsAsync(FlipDuration);

            _flipCount++;
        }
    }

    void OnDestroy() => ReleaseObjects();

    void Update()
    {
        // Flip animation time step
        _flipTime += Time.deltaTime / FlipDuration;

        // Foreground page insertion
        var fgTex1 = _flipCount < InsertionCount ? _fgFrames.front : _bgFrames.flip;
        var fgTex2 = _flipCount == InsertionCount ? _fgFrames.front : _bgFrames.sheet;
        var fgTime = _flipCount > 0 && _flipCount < InsertionCount ? 1 : _flipTime;

        // Rendering
        _bgParams.props.SetTexture("_Texture1", _bgFrames.flip);
        _fgParams.props.SetTexture("_Texture1", fgTex1);

        _bgParams.props.SetTexture("_Texture2", _bgFrames.sheet);
        _fgParams.props.SetTexture("_Texture2", fgTex2);

        _bgParams.props.SetFloat("_Progress", Mathf.Clamp01(_flipTime));
        _fgParams.props.SetFloat("_Progress", Mathf.Clamp01(fgTime));

        Graphics.RenderMesh(_bgParams.rparams, _pageMesh, 0, _bgParams.matrix);
        Graphics.RenderMesh(_fgParams.rparams, _pageMesh, 0, _fgParams.matrix);
    }
}

} // namespace Dcam2
