using UnityEngine;
using Unity.Properties;
using Unity.Mathematics;

namespace Dcam2 {

// FlipBook - Time management

public sealed partial class FlipBook
{
    #region Derived properties

    int QueueLength
      => (int)(_sequenceDuration / _sampleInterval);

    public double SequenceDuration
      => QueueLength * (double)_sampleInterval;

    public double LastPageDuration
      => SequenceDuration *
         math.pow(_sampleInterval / SequenceDuration, 1.0 / _easeOutPower);

    public double CurrentTime
      => Time.timeAsDouble;

    int CurrentSampleIndex
      => (int)(CurrentTime / _sampleInterval);

    public double CurrentPlayTime
      => CurrentTime - LastPageDuration - SequenceDuration;

    public int CurrentPlaySequenceIndex
      => (int)(CurrentPlayTime / SequenceDuration);

    #endregion

    #region Async methods

    async Awaitable WaitSampleIndex(int index)
    {
        while (CurrentSampleIndex < index)
            await Awaitable.NextFrameAsync();
    }

    public async Awaitable WaitPlaySequenceIndex(int index)
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
