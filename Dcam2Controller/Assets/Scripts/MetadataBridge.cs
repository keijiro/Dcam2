using UnityEngine;
using Klak.Ndi;

namespace Dcam2 {

public sealed class MetadataBridge : MonoBehaviour
{
    InputHandle _input;
    NdiSender _sender;

    Metadata MetadataFromInput
      => new Metadata { InputState = _input.InputState };

    void Start()
    {
        _input = GetComponent<InputHandle>();
        _sender = GetComponent<NdiSender>();
    }

    void Update()
      => _sender.metadata = MetadataFromInput.Serialize();
}

} // namespace Dcam2
