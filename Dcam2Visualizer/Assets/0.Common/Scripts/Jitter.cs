using UnityEngine;
using Unity.Mathematics;
using Klak.Math;
using Random = Unity.Mathematics.Random;

namespace Dcam2 {

public sealed class Jitter : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public float Amount { get; set; }

    #endregion

    #region Editable attributes

    [SerializeField] uint _seed = 32879;

    #endregion

    #region MonoBehaviour implementation

    Random _random;

    void Start()
      => _random = new Random(_seed);

    void Update()
      => transform.localPosition = _random.NextFloat3InSphere() * Amount;

    #endregion
}

} // namespace Dcam2
