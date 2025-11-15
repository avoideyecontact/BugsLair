using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CockroachDeath : MonoBehaviour
{
    [SerializeField] private HealthComponent _health;
    [SerializeField] private Animator _animator;
    [SerializeField] private CockroachAI _ai;
    [SerializeField] private SurfaceAligner _aligner;
    [SerializeField] private RandomDrop _drop;
    [SerializeField] private RandomDrop _gearDrop;

    [SerializeField] private AudioSource _sound;
    [SerializeField] private AudioSource _deathSound;

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
        _gearDrop.CreateDrop();
        _ai.Disable();
        _aligner.enabled = false;
        GetComponent<Collider>().enabled = false;

        _animator.SetTrigger("SwitchToRunning");
        _animator.applyRootMotion = true;

        Transform child = transform.GetChild(0);

        var ct = this.GetCancellationTokenOnDestroy();

        float randomRotation = Random.Range(-45, 45);
        _ = child.DOLocalRotate(new Vector3(0, randomRotation, 180), 1f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);
        await child.DOLocalMoveY(1f, 0.5f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);
        _sound.Stop();
        _deathSound.Play();
        await child.DOLocalMoveY(-0.5f, 0.5f).SetEase(Ease.InOutBack).SetRelative().WithCancellation(ct);

        float multiplier = 1f;
        await DOTween.To(() => multiplier, x => multiplier = x, 0f, 2f)
            .OnUpdate(() =>
            {
                _animator.SetFloat("RunMultiplier", multiplier);
            }).WithCancellation(ct);

        var deathTasks = new UniTask[3];
        deathTasks[0] = child.DOLocalMoveY(-0.65f, 3f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        deathTasks[1] = child.DOScale(0, 3f).SetEase(Ease.InOutSine).WithCancellation(ct);
        deathTasks[2] = child.DOShakeRotation(2f, 10f).WithCancellation(ct);

        await UniTask.WhenAll(deathTasks);

        Destroy(gameObject);
    }
}
