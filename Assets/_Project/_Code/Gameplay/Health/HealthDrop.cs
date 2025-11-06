using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    [SerializeField] int health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<HealthComponent>().Heal(health);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AnimationTask().Forget();
    }

    private async UniTask AnimationTask()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        Transform child = transform.GetChild(0);

        while (!ct.IsCancellationRequested)
        {
            var rotateTask = child.DORotate(new Vector3(0, 180, 0), 4f).SetEase(Ease.Linear).SetRelative().WithCancellation(ct);
            await child.DOMoveY(0.5f, 2f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
            await child.DOMoveY(-0.5f, 2f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
            await rotateTask;
        }
    }
}
