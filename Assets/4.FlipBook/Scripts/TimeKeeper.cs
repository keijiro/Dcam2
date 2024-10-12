using UnityEngine;
using Unity.Properties;
using Unity.Mathematics;

namespace Dcam2 {

public sealed class TimeKeeper : MonoBehaviour
{
    #region Editable attributes

    // These values should be recalculated, so don't use them directly.
    // Use the public properties instead.

    [SerializeField] float _pageInterval = 0.1f;
    [SerializeField] float _sequenceDuration = 1.5f;
    [SerializeField] float _easeOutPower = 2;

    #endregion

    #region Public properties

    public double PageInterval
      => _pageInterval;

    public int PagePerSequence
      => (int)(_sequenceDuration / _pageInterval);

    public double SequenceDuration
      => PagePerSequence * (double)_pageInterval;

    public double EaseOutPower
      => _easeOutPower;

    public double LastPageDuration
      => SequenceDuration *
         math.pow(PageInterval / SequenceDuration, 1.0 / _easeOutPower);

    public double CurrentTime
      => Time.timeAsDouble;

    public int CurrentPageIndex
      => (int)(CurrentTime / PageInterval);

    public double CurrentPlayTime
      => CurrentTime - LastPageDuration - SequenceDuration;

    public int CurrentPlaySequenceIndex
      => (int)(CurrentPlayTime / SequenceDuration);

    #endregion

    #region Async methods

    public async Awaitable WaitPageAsync(int index)
    {
        while (CurrentPageIndex < index)
            await Awaitable.NextFrameAsync();
    }

    public async Awaitable WaitPlaySequenceAsync(int index)
    {
        while (CurrentPlaySequenceIndex < index)
            await Awaitable.NextFrameAsync();
    }

    #endregion

    #if UNITY_EDITOR

    #region Editor UI 

    [CreateProperty]
    public string HintText
      => $"Last Page Duration: {LastPageDuration:0.000}";

    #endregion

    #endif
}

} // namespace Dcam2
