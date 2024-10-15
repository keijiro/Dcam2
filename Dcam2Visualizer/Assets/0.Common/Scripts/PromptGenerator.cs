using UnityEngine;

namespace Dcam2 {

public sealed class PromptGenerator : MonoBehaviour
{
    [SerializeField] string[] _basePrompts = null;
    [SerializeField] ImageGenerator _generator = null;

    (bool shift, int index) _select;

    void UpdatePrompt()
    {
        var i = (_select.shift ? 6 : 0) + _select.index;
        _generator.Prompt = _basePrompts[i % _basePrompts.Length];
    }

    public void SetBaseIndexShift(bool shift)
        => _select.shift = shift;

    public void SetBaseIndex(int index)
    {
        _select.index = index;
        UpdatePrompt();
    }
}

} // namespace Dcam2
