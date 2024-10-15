using UnityEngine;

namespace Dcam2 {

public sealed class PromptGenerator : MonoBehaviour
{
    #region Scene object reference

    [System.Serializable]
    struct EffectSuffix
    {
        public GameObject Target;
        public string Prompt;

        public EffectSuffix(GameObject target, string prompt)
          => (Target, Prompt) = (target, prompt);
    }

    [SerializeField] ImageGenerator _generator = null;
    [SerializeField] string _prefix = null;
    [SerializeField] string[] _bodies = null;
    [SerializeField] EffectSuffix[] _suffixes = null;

    #endregion

    #region Public properties

    public bool SelectorShift
      { get; set; }

    public int SelectorIndex
      { get => _selectorIndex; set => SetSelectorIndex(value); }

    #endregion

    #region Property backing members

    int _selectorIndex;

    void SetSelectorIndex(int index)
    {
        _selectorIndex = index;
        UpdatePrompt();
    }

    #endregion

    #region Prompt generator

    void UpdatePrompt()
    {
        var prompt = _prefix + " ";

        var idx = (SelectorShift ? 6 : 0) + _selectorIndex;
        prompt += _bodies[idx % _bodies.Length];

        for (var i = 0; i < _suffixes.Length; i++)
        {
            if (_suffixes[i].Target.activeSelf)
                prompt += " covered with " + _suffixes[i].Prompt;
        }

        _generator.Prompt = prompt;
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => UpdatePrompt();

    #endregion
}

} // namespace Dcam2
