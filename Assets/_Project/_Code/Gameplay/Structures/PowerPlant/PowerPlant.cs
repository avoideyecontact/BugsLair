using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PowerPlant : MonoBehaviour
{
    [SerializeField] private HealthComponent _fuseBoxHealth;

    [Header("Objects for animation")]
    [SerializeField] private Transform _cylinder1;
    [SerializeField] private Transform _cylinder2;
    [SerializeField] private Transform _wheel;

    [Header("Wires")]
    [SerializeField] private Transform _wires;
    [SerializeField] private Transform _wiresDamaged;

    private UniTask _animationTask;
    private bool _isAnimating;

    private void Start()
    {
        _fuseBoxHealth.OnDeath += OnFuseBoxDestruction;
        _wiresDamaged.gameObject.SetActive(false);
        _animationTask = StartAnimation();
    }

    private void OnFuseBoxDestruction()
    {
        _wires.gameObject.SetActive(false);
        _wiresDamaged.gameObject.SetActive(true);
        SlowDown().Forget();
    }

    private async UniTask StartAnimation()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _isAnimating = true;
        await _cylinder1.DOLocalMoveY(-1f, 0f).SetRelative().WithCancellation(ct);
        await _cylinder2.DOLocalMoveY(1f, 0f).SetRelative().WithCancellation(ct);

        var wheelTask = _wheel.DOLocalRotate(new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360)
            .SetRelative()
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .WithCancellation(ct);

        while (_isAnimating && !ct.IsCancellationRequested)
        {

            var cylinder1Task = _cylinder1.DOLocalMoveY(2f, 0.5f)
                .SetRelative()
                .SetEase(Ease.Linear)
                .WithCancellation(ct);

            var cylinder2Task = _cylinder2.DOLocalMoveY(-2f, 0.5f)
                .SetRelative()
                .SetEase(Ease.Linear)
                .WithCancellation(ct);

            await UniTask.WhenAll(cylinder1Task, cylinder2Task);

            var cylinder1Task2 = _cylinder1.DOLocalMoveY(-2f, 0.5f)
                .SetRelative()
                .SetEase(Ease.Linear)
                .WithCancellation(ct);

            var cylinder2Task2 = _cylinder2.DOLocalMoveY(2f, 0.5f)
                .SetRelative()
                .SetEase(Ease.Linear)
                .WithCancellation(ct);

            await UniTask.WhenAll(cylinder1Task2, cylinder2Task2);
        }

        _ = _cylinder1.DOLocalMoveY(2f, 0.5f).SetEase(Ease.Linear).WithCancellation(ct);
        await _cylinder2.DOLocalMoveY(2f, 0.5f).SetEase(Ease.Linear).WithCancellation(ct);
    }

    private async UniTask StopAnimationTask()
    {
        _isAnimating = false;
        await _animationTask;
    }

    private async UniTask SlowDown()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitForSeconds(1, cancellationToken: ct);
        await StopAnimationTask();

        var rotationTask = _wheel.DOLocalRotate(new Vector3(360, 0, 0), 6f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .SetRelative();

        _ = _cylinder1.DOLocalMoveY(1f, 1f).SetEase(Ease.Linear).SetRelative().WithCancellation(ct);
        await _cylinder2.DOLocalMoveY(-1f, 1f).SetEase(Ease.Linear).SetRelative().WithCancellation(ct);
        _ = _cylinder1.DOLocalMoveY(-0.5f, 1f).SetEase(Ease.OutSine).SetRelative().WithCancellation(ct);
        await _cylinder2.DOLocalMoveY(0.5f, 1f).SetEase(Ease.OutSine).SetRelative().WithCancellation(ct);
        await rotationTask;

        _cylinder1.DOKill();
        _cylinder2.DOKill();
        _wheel.DOKill();
    }
}
