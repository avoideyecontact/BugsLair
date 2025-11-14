using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SpiderDeath : MonoBehaviour
{
    [SerializeField] private HealthComponent _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private Spider _ai;
    [SerializeField] private SurfaceAligner _aligner;
    [SerializeField] private RandomDrop _drop;

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
        _drop.CreateDrop();
        _ai.Deactivate();
        _aligner.enabled = false;
        GetComponent<Collider>().enabled = false;

        _animator.SetTrigger("Death");
        _animator.applyRootMotion = true;

        Transform child = transform.GetChild(0);

        var ct = this.GetCancellationTokenOnDestroy();

        float randomRotation = Random.Range(-45, 45);
        _ = child.DOLocalRotate(new Vector3(0, randomRotation, 180), 1f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);
        await child.DOLocalMoveY(1f, 0.5f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);
        await child.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);

        await UniTask.WaitForSeconds(1f, cancellationToken: ct);

        var deathTasks = new UniTask[3];
        deathTasks[0] = child.DOLocalMoveY(-0.65f, 3f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        deathTasks[1] = child.DOScale(0, 3f).SetEase(Ease.InOutSine).WithCancellation(ct);
        deathTasks[2] = child.DOShakeRotation(2f, 10f).WithCancellation(ct);

        await UniTask.WhenAll(deathTasks);

        Destroy(gameObject);
    }
}
