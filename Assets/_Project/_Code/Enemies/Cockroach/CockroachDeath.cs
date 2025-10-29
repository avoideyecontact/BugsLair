using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CockroachDeath : MonoBehaviour
{
    [SerializeField] private HealthComponent _health;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _health.OnDeath += InitiateDeath;
    }

    private void OnDestroy()
    {
        _health.OnDeath -= InitiateDeath;
    }

    public void InitiateDeath()
    {
        _health.OnDeath -= InitiateDeath;
        DeathTask().Forget();
    }

    private async UniTaskVoid DeathTask()
    {
        GetComponent<Collider>().enabled = false;

        _animator.SetTrigger("SwitchToRunning");
        _animator.applyRootMotion = true;

        var ct = this.GetCancellationTokenOnDestroy();

        float randomRotation = Random.Range(-45, 45);
        _ = transform.DORotate(new Vector3(0, randomRotation, 180), 1f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);
        await transform.DOLocalMoveY(1f, 0.5f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);
        await transform.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);

        float multiplier = 1f;
        await DOTween.To(() => multiplier, x => multiplier = x, 0f, 2f)
            .OnUpdate(() =>
            {
                _animator.SetFloat("RunMultiplier", multiplier);
            }).WithCancellation(ct);

        var deathTasks = new UniTask[3];
        deathTasks[0] = transform.DOLocalMoveY(-0.65f, 3f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        deathTasks[1] = transform.DOScale(0, 3f).SetEase(Ease.InOutSine).WithCancellation(ct);
        deathTasks[2] = transform.DOShakeRotation(2f, 10f).WithCancellation(ct);

        await UniTask.WhenAll(deathTasks);

        Destroy(gameObject);
    }
}
