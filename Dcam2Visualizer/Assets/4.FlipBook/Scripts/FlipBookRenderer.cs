using UnityEngine;
using Unity.Mathematics;

namespace Dcam2 {

public sealed class FlipBookRenderer : MonoBehaviour
{
    #region Public property

    [field:SerializeField] public bool EaseOut { get; set; } = true;
    [field:SerializeField] public float MotionBlur { get; set; } = 0.2f;

    #endregion

    #region Scene object reference

    [SerializeField] PageSequencer _sequencer = null;
    [SerializeField] TimeKeeper _timeKeeper = null;

    #endregion

    #region Project asset references

    [SerializeField, HideInInspector] Mesh _pageMesh = null;
    [SerializeField, HideInInspector] Material _pageMaterial = null;

    #endregion

    #region MonoBehaviour implementation

    PageDrawer _bg, _fg;

    void Start()
    {
        _bg = new PageDrawer(_pageMesh, _pageMaterial, gameObject.layer,
                             z: 0.01f, scale: 3, occlusion: 0.85f);

        _fg = new PageDrawer(_pageMesh, _pageMaterial, gameObject.layer,
                             z: 0, scale: 1, occlusion: 0);
    }

    void Update()
    {
        var time = _timeKeeper.PlayerTime;

        // Sequence progress parameters;
        var t_bg = time.SequenceProgress;
        var t_fg = EaseOut ? time.SequenceProgressEased : t_bg;

        // Page texture pairs
        var qidx = time.SequenceIndex % 2;
        var bg_base = _sequencer.GetPage(qidx, (int)t_bg - 1);
        var bg_flip = _sequencer.GetPage(qidx, (int)t_bg    );
        var fg_base = _sequencer.GetPage(qidx, (int)t_fg - 1);
        var fg_flip = _sequencer.GetPage(qidx, (int)t_fg    );

        // Derivative of the progress parameter (for motion blur)
        var ddt = EaseOut ? (float)time.SequenceProgressEasedDerivative : 1;

        // Page rendering
        _bg.Render(bg_base, bg_flip, (float)math.frac(t_bg), MotionBlur);
        _fg.Render(fg_base, fg_flip, (float)math.frac(t_fg), MotionBlur * ddt);
    }

    #endregion
}

} // namespace Dcam2
