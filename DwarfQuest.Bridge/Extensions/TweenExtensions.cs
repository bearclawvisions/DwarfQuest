using Godot;

namespace DwarfQuest.Bridge.Extensions;

public static class TweenExtensions
{
    private const float CountDuration = 1.5f;
    
    public static void CountUpAnimation(this Label label, int to, int from = 0)
    {
        var tween = label.CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Cubic);
        tween.TweenMethod(Callable.From((double value) => 
        {
            label.Text = Mathf.RoundToInt((float)value).ToString();
        }), from, to, CountDuration);
    }
}