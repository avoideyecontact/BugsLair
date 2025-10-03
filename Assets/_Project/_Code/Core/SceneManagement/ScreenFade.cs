using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public async UniTask FadeInAsync(float duration = 1f)
    {
        _canvasGroup.alpha = 0f;
        await _canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.InOutSine)
            .ToUniTask();
    }

    public async UniTask FadeOutAsync(float duration = 1f)
    {
        _canvasGroup.alpha = 1f;
        await _canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.InOutSine)
            .ToUniTask();
    }
}
