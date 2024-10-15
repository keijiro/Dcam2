using UnityEngine;
using UnityEngine.Events;

namespace Dcam2 {

public sealed class RemoteToggleToBoolValue : MonoBehaviour
{
    [SerializeField] int _toggleIndex = 0;
    [SerializeField] UnityEvent<bool> _event = null;

    InputHandle _input;
    bool _prev;

    void Start()
      => _input = FindFirstObjectByType<InputHandle>();

    void Update()
    {
        var current = _input.GetToggle(_toggleIndex);
        if (current == _prev) return;
        _event.Invoke(current);
        _prev = current;
    }
}

} // namespace Dcam2
