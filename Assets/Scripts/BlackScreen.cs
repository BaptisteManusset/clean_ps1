using System;
using DG.Tweening;
using UnityEngine.UI;

public class BlackScreen : SceneSingleton<BlackScreen>
{
    public Image image;
    public Ease ease = Ease.InSine;
    public float duration = 0.2f;

    public event Action OnFadeStart;
    public event Action OnFadeEnd;
    public event Action OnFadeOpaque;

    private void Start()
    {
    }

    [EditorButton]
    public void Black()
    {
        OnFadeStart?.Invoke();
        image.DOFade(1, duration).SetEase(ease);
        OnFadeOpaque?.Invoke();
        OnFadeEnd?.Invoke();
    }

    [EditorButton]
    public void Clear()
    {
        OnFadeStart?.Invoke();
        image.DOFade(0, duration).SetEase(ease);
        OnFadeEnd?.Invoke();
    }

    [EditorButton]
    public void Fade()
    {
        Sequence sequence = DOTween.Sequence();

        OnFadeStart?.Invoke();
        sequence.Append(image.DOFade(1, duration).SetEase(ease));
        sequence.AppendCallback(() => { OnFadeOpaque?.Invoke(); });
        sequence.AppendInterval(duration);
        sequence.Append(image.DOFade(0, duration).SetEase(ease));
        OnFadeEnd?.Invoke();
    }
}