using UnityEngine;
using UnityEngine.UIElements;

namespace Dcam2 {

public sealed class DebugViewController : MonoBehaviour
{
    [SerializeField] PageSequencer _sequencer = null;

    void Update()
    {
        var p1 = _sequencer.GetPage(1, -1);
        var p2 = _sequencer.GetPage(0, -1);

        if (p1 == null || p2 == null) return;

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Q("queue1").style.backgroundImage = Background.FromRenderTexture(p1);
        root.Q("queue2").style.backgroundImage = Background.FromRenderTexture(p2);
    }
}

} // namespace Dcam2
