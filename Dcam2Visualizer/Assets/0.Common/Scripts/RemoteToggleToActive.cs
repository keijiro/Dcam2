using UnityEngine;
using UnityEngine.Events;

namespace Dcam2 {

public sealed class RemoteToggleToActive : MonoBehaviour
{
    [SerializeField] int _toggleIndex = 0;
    [SerializeField] GameObject _target = null;

    InputHandle _input;

    void Start()
      => _input = FindFirstObjectByType<InputHandle>();

    void Update()
    {
        var flag = _input.GetToggle(_toggleIndex);
        if (flag == _target.activeSelf) return;
        _target.SetActive(flag);
    }
}

} // namespace Dcam2
