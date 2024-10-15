using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;
using Klak.Math;
using Klak.Motion;

namespace Dcam2 {

public sealed class CameraController : MonoBehaviour
{
    #region Scene object references

    [SerializeField] TimeKeeper _timeKeeper = null;
    [SerializeField] Transform _pivot = null;
    [SerializeField] Transform _arm = null;
    [SerializeField] Jitter _shaker = null;

    #endregion

    #region Public properties

    [field:Space, SerializeField]
    public float3 PositionRange { get; set; } = 0.05f;

    [field:SerializeField]
    public float3 RotationRange { get; set; } = math.float3(10, 10, 15);

    [field:SerializeField]
    public float DistanceRange { get; set; } = 0.5f;

    [field:Space, SerializeField]
    public float BackScale { get; set; } = 2;

    [field:SerializeField]
    public float2 TweenSpeed { get; set; } = math.float2(3, 20);

    [field:SerializeField]
    public float ShakeAmount { get; set; } = 0.1f;

    [field:Space, SerializeField]
    public uint Seed { get; set; } = 8943;

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
        for (var (time, rand) = (_timeKeeper.PlayerTime, new Random(Seed));;)
        {
            // New angle
            var pos = rand.NextFloat3(-PositionRange, PositionRange);
            var rot = rand.NextFloat3(-RotationRange, RotationRange);
            var dist = rand.NextFloat (-DistanceRange, 0);

            // Slow moving away
            for (var next = time.SequenceLastPageTime; time.Seconds < next;)
            {
                UpdateTransforms(pos, rot, dist, BackScale, TweenSpeed.x);
                await Awaitable.NextFrameAsync();
            }

            // Shake initiation
            _shaker.Amount = ShakeAmount;

            // Fast zoom in
            for (var next = time.NextSequenceStartTime; time.Seconds < next;)
            {
                UpdateTransforms(pos, rot, dist, 1, TweenSpeed.y);
                await Awaitable.NextFrameAsync();
            }
        }
    }

    #endregion
}

} // namespace Dcam2
