using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

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
        _animator.SetTrigger("Death");
        await UniTask.WaitForSeconds(1f);
        _animator.applyRootMotion = true;

        var deathTasks = new UniTask[3];
        deathTasks[0] = transform.DOLocalMoveY(-0.5f, 2f).SetEase(Ease.InOutSine).SetRelative().ToUniTask();
        deathTasks[1] = transform.DOScale(0, 1.5f).SetEase(Ease.InOutSine).ToUniTask();
        deathTasks[2] = transform.DOShakeRotation(2f, 10f).ToUniTask();

        await UniTask.WhenAll(deathTasks);

        Destroy(gameObject);
    }
}
