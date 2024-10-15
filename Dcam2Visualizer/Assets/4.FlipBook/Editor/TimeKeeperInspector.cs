using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Dcam2 {

[CustomEditor(typeof(TimeKeeper))]
public class TimeKeeperInspector : Editor
{
    public VisualTreeAsset _xml;

    public override VisualElement CreateInspectorGUI()
    {
        _xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
          ("Assets/4.FlipBook/Editor/TimeKeeperInspector.uxml");
        var root = _xml.Instantiate();
        root.dataSource = target;
        return root;
    }
}

} // namespace Dcam2
