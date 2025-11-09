using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AbilityDrop : MonoBehaviour
{
    [SerializeField] private AbilityType _abilityType;
    public AbilityType GetAbilityType => _abilityType;

    private void Start()
    {
        AnimationTask().Forget();
    }

    private async UniTask AnimationTask()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        Transform child0 = transform.GetChild(0);
        Transform child1 = transform.GetChild(1);

        while (!ct.IsCancellationRequested)
        {
            var rotateTask1 = child0.DORotate(new Vector3(0, 180, 0), 4f).SetEase(Ease.Linear).SetRelative().WithCancellation(ct);
            var rotateTask2 = child1.DORotate(new Vector3(0, -180, 0), 4f).SetEase(Ease.Linear).SetRelative().WithCancellation(ct);
            await child1.DOMoveY(0.5f, 2f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
            await child1.DOMoveY(-0.5f, 2f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
            await rotateTask1;
            await rotateTask2;
        }
    }
}
