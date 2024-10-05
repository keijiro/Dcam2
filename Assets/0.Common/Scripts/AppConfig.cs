using UnityEngine;

namespace Dcam2 {

public sealed class AppConfig : MonoBehaviour
{
    [SerializeField] int _targetFpsEditor = 60;
    [SerializeField] int _targetFpsPlayer = -1;

    void Start()
      => Application.targetFrameRate =
           Application.isEditor ? _targetFpsEditor : _targetFpsPlayer;
}

} // namespace Dcam2
