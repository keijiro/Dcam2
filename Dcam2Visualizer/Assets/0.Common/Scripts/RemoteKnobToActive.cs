using UnityEngine;
using UnityEngine.Events;

namespace Dcam2 {

public sealed class RemoteKnobToActive : MonoBehaviour
{
    [SerializeField] int _knobIndex = 0;
    [SerializeField] float _threshold = 0.5f;
    [SerializeField] GameObject _target = null;

    InputHandle _input;

    void Start()
      => _input = FindFirstObjectByType<InputHandle>();

    void Update()
    {
        var flag = _input.GetKnob(_knobIndex) > _threshold;
        if (flag == _target.activeSelf) return;
        _target.SetActive(flag);
    }
}

} // namespace Dcam2
