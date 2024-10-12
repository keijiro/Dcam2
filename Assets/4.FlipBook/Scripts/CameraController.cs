using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
using Klak.Math;
using Klak.Motion;

namespace Dcam2 {

public sealed class CameraController : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] TimeKeeper _timeKeeper = null;
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

    void UpdateTransforms
      (float3 pos, float3 angles, float dist, float scale, float speed)
    {
        // Scale factor
        pos *= scale;
        angles *= scale;
        dist *= scale;

        var rot = quaternion.EulerXZY(math.radians(angles));

        // Position / rotation
        _pivot.localPosition = ExpTween.Step(_pivot.localPosition, pos, speed);
        _pivot.localRotation = ExpTween.Step(_pivot.localRotation, rot, speed);

        // Distance
        var z = _arm.localPosition.z;
        z = ExpTween.Step(z, dist, speed);
        _arm.localPosition = math.float3(0, 0, z);

        // Shake
        _shaker.Amount = ExpTween.Step(_shaker.Amount, 0, speed);
    }

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        for (var (time, rand) = (_timeKeeper.PlayerTime, new Random(_seed));;)
        {
            // New angle
            var pos = rand.NextFloat3(-_positionRange, _positionRange);
            var rot = rand.NextFloat3(-_rotationRange, _rotationRange);
            var dist = rand.NextFloat (-_distanceRange, 0);

            // Slow moving away
            for (var next = time.SequenceLastPageTime; time.Seconds < next;)
            {
                UpdateTransforms(pos, rot, dist, _backScale, _tweenSpeed.x);
                await Awaitable.NextFrameAsync();
            }

            // Shake initiation
            _shaker.Amount = _shakeAmount;

            // Fast zoom in
            for (var next = time.NextSequenceStartTime; time.Seconds < next;)
            {
                UpdateTransforms(pos, rot, dist, 1, _tweenSpeed.y);
                await Awaitable.NextFrameAsync();
            }
        }
    }

    #endregion
}

} // namespace Dcam2
