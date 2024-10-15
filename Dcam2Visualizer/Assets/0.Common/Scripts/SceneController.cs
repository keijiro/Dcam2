using UnityEngine;

namespace Dcam2 {

public sealed class SceneController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public bool EnableGenerator { get; set; } = true;

    #endregion

    #region Scene object references

    [SerializeField] TimeKeeper _timeKeeper = null;
    [SerializeField] PageSequencer _sequencer = null;
    [SerializeField] FlipBookRenderer _flipBook = null;
    [SerializeField] CameraController _camera = null;

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        for (var time = _timeKeeper.PlayerTime;;)
        {
            await time.WaitSequenceAsync(time.SequenceIndex + 1);

            _timeKeeper.EaseOutPower = EnableGenerator ? 2.73f : 1.4f;
            _sequencer.EnableGenerator = EnableGenerator;
            _flipBook.MotionBlur = EnableGenerator ? 0.2f : 0.1f;
            _camera.ShakeAmount = EnableGenerator ? 0.1f : 0;
            _camera.BackScale = EnableGenerator ? 2 : 1;
        }
    }

    #endregion
}

} // namespace Dcam2
