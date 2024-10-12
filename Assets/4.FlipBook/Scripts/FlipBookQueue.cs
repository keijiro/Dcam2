using UnityEngine;

namespace Dcam2 {

// FlipBook - Page queue implementation

public sealed partial class FlipBook
{
    RenderTexture[][] _queue;

    void InitializeQueue()
    {
        _queue = new RenderTexture[2][];
        _queue[0] = new RenderTexture[_time.PagePerSequence];
        _queue[1] = new RenderTexture[_time.PagePerSequence];

        for (var i = 0; i < _time.PagePerSequence; i++)
        {
            _queue[0][i] = new RenderTexture(ImageWidth, ImageHeight, 0);
            _queue[1][i] = new RenderTexture(ImageWidth, ImageHeight, 0);
        }
    }

    void FinalizeQueue()
    {
        for (var i = 0; i < _time.PagePerSequence; i++)
        {
            Destroy(_queue[0][i]);
            Destroy(_queue[1][i]);
        }
    }

    // Page reference allowing negative indices
    RenderTexture GetPage(int qidx, int pidx)
    {
        qidx += 2;
        pidx += _time.PagePerSequence * 2;
        return _queue[qidx % 2][pidx % _time.PagePerSequence];
    }
}

} // namespace Dcam2
