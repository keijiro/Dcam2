using UnityEngine;

namespace Dcam2 {

// FlipBook - Renderer implementation

public sealed partial class FlipBook
{
    // Background/foreground
    (MaterialPropertyBlock props, RenderParams rparams, Matrix4x4 matrix) _bg;
    (MaterialPropertyBlock props, RenderParams rparams, Matrix4x4 matrix) _fg;

    // Transform matrix calculation
    static Matrix4x4 MakePageTransform(float z, float scale)
      => Matrix4x4.TRS
           (Vector3.forward * z, Quaternion.identity,
            new Vector3(1, (float)ImageHeight / ImageWidth, 1) * scale);

    // RenderParams factory
    RenderParams NewRenderParams(MaterialPropertyBlock props)
      => new RenderParams(_pageMaterial)
           { matProps = props, layer = gameObject.layer };

    void InitializeRenderer()
    {
        _bg.props = new MaterialPropertyBlock();
        _fg.props = new MaterialPropertyBlock();

        _bg.rparams = NewRenderParams(_bg.props);
        _fg.rparams = NewRenderParams(_fg.props);

        _bg.matrix = MakePageTransform(0.01f, 3);
        _fg.matrix = MakePageTransform(0, 1);

        _bg.props.SetFloat(ShaderID.Occlusion, 0.85f);
        _bg.props.SetFloat(ShaderID.Aspect, (float)ImageWidth / ImageHeight);
    }

    void FinalizeRenderer()
    {
    }

    void RenderBackPage
      ((RenderTexture baseRT, RenderTexture flipRT) textures,
       double progress, double blur)
    {
        _bg.props.SetTexture(ShaderID.BaseTex, textures.baseRT);
        _bg.props.SetTexture(ShaderID.FlipTex, textures.flipRT);
        _bg.props.SetFloat(ShaderID.Progress, (float)progress);
        _bg.props.SetFloat(ShaderID.Blur, _motionBlur * (float)blur);
        Graphics.RenderMesh(_bg.rparams, _pageMesh, 0, _bg.matrix);
    }

    void RenderFrontPage
      ((RenderTexture baseRT, RenderTexture flipRT) textures,
       double progress, double blur)
    {
        _fg.props.SetTexture(ShaderID.BaseTex, textures.baseRT);
        _fg.props.SetTexture(ShaderID.FlipTex, textures.flipRT);
        _fg.props.SetFloat(ShaderID.Progress, (float)progress);
        _fg.props.SetFloat(ShaderID.Blur, _motionBlur * (float)blur);
        Graphics.RenderMesh(_fg.rparams, _pageMesh, 0, _fg.matrix);
    }
}

} // namespace Dcam2
