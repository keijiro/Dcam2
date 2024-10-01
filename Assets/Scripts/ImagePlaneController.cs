using UnityEngine;
using Klak.TestTools;

namespace Dcam2 {

public sealed class ImagePlaneController : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;

    MaterialPropertyBlock _prop;

    void Start()
      => _prop = new MaterialPropertyBlock();

    void Update()
    {
        _prop.SetTexture("_MainTex", _source.AsTexture);
        GetComponent<Renderer>().SetPropertyBlock(_prop);
    }
}

} // namespace Dcam2
