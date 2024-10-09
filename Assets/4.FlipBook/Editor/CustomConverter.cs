using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dcam2 {

public static class CustomConverter
{
    [UnityEditor.InitializeOnLoadMethod]
    public static void RegisterConverters()
    {
        var grp = new ConverterGroup("Double to String (Three Decimal)");
        grp.AddConverter((ref double v) => $"{v:0.000}");
        ConverterGroups.RegisterConverterGroup(grp);
    }
}

} // namespace Dcam2
