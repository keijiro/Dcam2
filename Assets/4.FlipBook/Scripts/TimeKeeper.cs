using UnityEngine;
using Unity.Properties;
using Unity.Mathematics;

namespace Dcam2 {

public sealed class TimeKeeper : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] float _pageInterval = 0.1f;
    [SerializeField] float _sequenceDuration = 1.5f;
    [SerializeField] float _easeOutPower = 2;

    #endregion

    #region Public properties

    // Recorder adapter
    public RecorderTime RecorderTime
        => new RecorderTime(this);

    // Player adapter
    public PlayerTime PlayerTime
        => new PlayerTime(this);

    // The page interval (the duration between pages)
    public double PageInterval
      => _pageInterval;

    // The page count per sequence
    public int PagePerSequence
      => (int)(_sequenceDuration / _pageInterval);

    // The duration of a sequence (recalculated)
    public double SequenceDuration
      => PagePerSequence * (double)_pageInterval;

    // The power parameter for the ease-out function
    public double EaseOutPower
      => _easeOutPower;

    // The duration of the last page with ease-out
    public double LastPageDuration
      => SequenceDuration *
         math.pow(PageInterval / SequenceDuration, 1.0 / EaseOutPower);

    #endregion

    #if UNITY_EDITOR

    #region Editor UI 

    [CreateProperty]
    public string HintText
      => $"Last Page Duration: {LastPageDuration:0.000}";

    #endregion

    #endif
}

// Recorder time adapter for TimeKeeper
public struct RecorderTime
{
    TimeKeeper _time;

    public RecorderTime(TimeKeeper time)
      => _time = time;

    // The current time in seconds
    public double Seconds
      => Time.timeAsDouble;

    // The index of the current page
    public int PageIndex
      => (int)(Seconds / _time.PageInterval);

    // Page awaiter
    public async Awaitable WaitPageAsync(int index)
    {
        while (PageIndex < index) await Awaitable.NextFrameAsync();
    }
}

// Player time adapter for TimeKeeper
public struct PlayerTime
{
    TimeKeeper _time;

    public PlayerTime(TimeKeeper time)
      => _time = time;

    // The current time in seconds
    public double Seconds
      => Time.timeAsDouble - _time.LastPageDuration - _time.SequenceDuration;

    // The index of the current sequence
    public int SequenceIndex
      => (int)(Seconds / _time.SequenceDuration);

    // The start time of the current sequence
    public double SequenceStartTime
      => SequenceIndex * _time.SequenceDuration;

    // The start time of the last page in the current sequence
    public double SequenceLastPageTime
      => SequenceStartTime + _time.SequenceDuration - _time.LastPageDuration;

    // The start time of the next sequence
    public double NextSequenceStartTime
      => SequenceStartTime + _time.SequenceDuration;

    // The progress parameter in a sequence
    // 0.0 = Only the last page in the last sequence is shown.
    // 0.5 = 50% of the 1st page is overlapped on the last page.
    // 1.0 = The 1st page is fully shown.
    // ...
    public double SequenceProgress
      => SequenceProgressNormalized * _time.PagePerSequence;

    // The normalized progress parameter in a sequence
    // 0.0     = Sequence start
    // 1.0 - e = Sequence end
    // 1.0     = Next sequence start
    public double SequenceProgressNormalized
      => math.frac(Seconds / _time.SequenceDuration);

    // SequenceProgress with ease-out
    public double SequenceProgressEased
      => SequenceProgressEasedNormalized * _time.PagePerSequence;

    // SequenceProgressNormalized with ease-out
    public double SequenceProgressEasedNormalized
      => 1 - math.pow(1 - SequenceProgressNormalized, _time.EaseOutPower);

    // The derivative of the progress parameter (for motion blur)
    public double SequenceProgressEasedDerivative
      => _time.EaseOutPower *
           math.pow(1 - SequenceProgressNormalized, _time.EaseOutPower - 1);

    // Sequence awaiter
    public async Awaitable WaitSequenceAsync(int index)
    {
        while (SequenceIndex < index) await Awaitable.NextFrameAsync();
    }
}

} // namespace Dcam2
