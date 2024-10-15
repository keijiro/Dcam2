using UnityEngine;
using Lasp;

namespace Dcam2 {

public sealed class FlashGlitchController : MonoBehaviour
{
    #region Scene/project object references

    [SerializeField] public AudioLevelTracker _audio1 = null; 
    [SerializeField] public AudioLevelTracker _audio2 = null; 
    [SerializeField] public Texture _source = null;

    #endregion

    #region Public properties

    public float Amplitude1 { get; set; } = 1;
    public float Amplitude2 { get; set; } = 1;

    #endregion

    #region Private members

    MaterialPropertyBlock _block;

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        _block = new MaterialPropertyBlock();

        _block.SetTexture("_MainTex", _source);

        while (true)
        {
            _block.SetFloat("_Seed", Random.Range(0, 100000));
            _block.SetFloat("_Hue", Random.value);
            await Awaitable.WaitForSecondsAsync(Random.Range(0.01f, 0.2f));
        }
    }

    void LateUpdate()
    {
        var l1 = _audio1.normalizedLevel * Random.value * Amplitude1;
        var l2 = _audio2.normalizedLevel * Random.value * Amplitude2;

        _block.SetFloat("_Effect1", l1);
        _block.SetFloat("_Effect2", l2);

        GetComponent<Renderer>().SetPropertyBlock(_block);
    }

    #endregion
}

} // namespace Dcam2
