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

    double SequenceDuration
      => QueueLength * (double)_sampleInterval;

    double LastPageDuration
      => SequenceDuration *
         math.pow(_sampleInterval / SequenceDuration, 1.0 / _easeOutPower);

    double CurrentTime
      => Time.timeAsDouble;

    int CurrentSampleIndex
      => (int)(CurrentTime / _sampleInterval);

    double CurrentPlayTime
      => CurrentTime - LastPageDuration - SequenceDuration;

    #endregion

    #region Async methods

    async Awaitable WaitSampleIndex(int index)
    {
        while (CurrentSampleIndex < index) await Awaitable.NextFrameAsync();
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
