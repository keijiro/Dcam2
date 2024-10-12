using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
using Klak.Math;
using Klak.Motion;

namespace Dcam2 {

public sealed class CameraController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] TimeKeeper _time = null;
    [Space]
    [SerializeField] Transform _pivot = null;
    [SerializeField] Transform _arm = null;
    [SerializeField] Jitter _shaker = null;
    [Space]
    [SerializeField] float3 _positionRange = 0.1f;
    [SerializeField] float3 _rotationRange = 30;
    [SerializeField] float _distanceRange = 0.4f;
    [Space]
    [SerializeField] float _backScale = 2;
    [SerializeField] float2 _tweenSpeed = math.float2(3, 10);
    [SerializeField] float _shakeAmount = 0.1f;
    [Space]
    [SerializeField] uint _seed = 8943;

    #endregion

    #region Private members

    void UpdateTransforms(float3 pos, float3 angles, float dist, float speed)
    {
        var rot = quaternion.EulerXZY(math.radians(angles));
        _pivot.localPosition = ExpTween.Step(_pivot.localPosition, pos, speed);
        _pivot.localRotation = ExpTween.Step(_pivot.localRotation, rot, speed);

        var z = _arm.localPosition.z;
        z = ExpTween.Step(z, dist, speed);
        _arm.localPosition = math.float3(0, 0, z);

        _shaker.Amount = ExpTween.Step(_shaker.Amount, 0, speed);
    }

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        for (var rand = new Random(_seed);;)
        {
            var pos = rand.NextFloat3(-_positionRange, _positionRange);
            var rot = rand.NextFloat3(-_rotationRange, _rotationRange);
            var dist = rand.NextFloat (-_distanceRange, 0);

            var t_0 = _time.SequenceDuration * _time.CurrentPlaySequenceIndex;

            var t_1 = t_0 + _time.SequenceDuration - _time.LastPageDuration;
            while (_time.CurrentPlayTime < t_1)
            {
                UpdateTransforms(pos * _backScale, rot * _backScale, dist * _backScale, _tweenSpeed.x);
                await Awaitable.NextFrameAsync();
            }

            _shaker.Amount = _shakeAmount;

            var t_2 = t_0 + _time.SequenceDuration;
            while (_time.CurrentPlayTime < t_2)
            {
                UpdateTransforms(pos, rot, dist, _tweenSpeed.y);
                await Awaitable.NextFrameAsync();
            }
        }
    }

    #endregion
}

} // namespace Dcam2
