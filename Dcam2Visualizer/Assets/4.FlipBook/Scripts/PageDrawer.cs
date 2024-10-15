using UnityEngine;

namespace Dcam2 {

public sealed class PageDrawer
{
    Mesh _mesh;
    MaterialPropertyBlock _props;
    RenderParams _rparams;
    Matrix4x4 _matrix;

    public PageDrawer(Mesh mesh, Material material, int layer,
                      float z = 0, float scale = 1, float occlusion = 0)
    {
        _mesh = mesh;

        _props = new MaterialPropertyBlock();
        _props.SetFloat(ShaderID.Occlusion, occlusion);
        _props.SetFloat(ShaderID.Aspect, ImageGenerator.AspectRatio);

        _rparams = new RenderParams(material);
        _rparams.layer = layer;
        _rparams.matProps = _props;

        var sv = new Vector3(1, 1.0f / ImageGenerator.AspectRatio, 1) * scale;
        _matrix = Matrix4x4.TRS(Vector3.forward * z, Quaternion.identity, sv);
    }

    public void Render(RenderTexture baseRT, RenderTexture flipRT,
                       float progress, float blur)
    {
        _props.SetTexture(ShaderID.BaseTex, baseRT);
        _props.SetTexture(ShaderID.FlipTex, flipRT);
        _props.SetFloat(ShaderID.Progress, progress);
        _props.SetFloat(ShaderID.Blur, blur);
        Graphics.RenderMesh(_rparams, _mesh, 0, _matrix);
    }
}

} // namespace Dcam2
