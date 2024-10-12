using UnityEngine;

namespace Dcam2 {

public sealed class PageSequencer : MonoBehaviour
{
    #region Scene object references

    [SerializeField] TimeKeeper _timeKeeper = null;
    [SerializeField] RenderTexture _source = null;
    [SerializeField] ImageGenerator _generator = null;

    #endregion

    #region Private members

    int QueueLength => _timeKeeper.PagePerSequence;

    RenderTexture[][] _queue;

    RenderTexture NewPageRT()
      => new RenderTexture(ImageGenerator.Width, ImageGenerator.Height, 0);

    #endregion

    #region Public method

    // Page reference allowing negative indices
    public RenderTexture GetPage(int qidx, int pidx)
    {
        qidx += 2;
        pidx += QueueLength * 2;
        return _queue[qidx % 2][pidx % QueueLength];
    }

    #endregion

    #region MonoBehaviour implementation

    async Awaitable Start()
    {
        var time = _timeKeeper.RecorderTime;

        // Page queue initialization
        _queue = new RenderTexture[2][];

        _queue[0] = new RenderTexture[QueueLength];
        _queue[1] = new RenderTexture[QueueLength];

        for (var i = 0; i < QueueLength; i++)
        {
            _queue[0][i] = NewPageRT();
            _queue[1][i] = NewPageRT();
        }

        // Page recording loop
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
                    task = _generator.RunGeneratorAsync(page, page);
                else
                    Debug.Log("Previous generator task not completed");
            }
        }
    }

    void OnDestroy()
    {
        for (var i = 0; i < QueueLength; i++)
        {
            Destroy(_queue[0][i]);
            Destroy(_queue[1][i]);
        }
    }

    #endregion
}

} // namespace Dcam2
