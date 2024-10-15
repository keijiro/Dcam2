using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

namespace Dcam2 {

[AddComponentMenu("VFX/Property Binders/Dcam2/BodyPix Keypoint Binder")]
[VFXBinder("Dcam2/BodyPix Keypoint")]
public class BodyPixKeypointBinder : VFXBinderBase
{
    [VFXPropertyBinding("UnityEngine.GraphicsBuffer")]
    public ExposedProperty Property = "KeypointBuffer";
    public Prefilter Target = null;

    public override bool IsValid(VisualEffect component)
      => Target != null && component.HasGraphicsBuffer(Property);

    public override void UpdateBinding(VisualEffect component)
      => component.SetGraphicsBuffer(Property, Target.KeypointBuffer);

    public override string ToString()
      => $"Keypoints : '{Property}' -> {Target?.name ?? "(null)"}";
}

} // namespace Dcam2
