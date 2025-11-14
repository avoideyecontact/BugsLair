using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class SpiderLair : MonoBehaviour
{
    [SerializeField] private Transform _door;
    [SerializeField] private Spider[] _spiders;

    private bool _wasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_wasTriggered) return;

        if (other.CompareTag("Player"))
        {
            StartSpiderFight().Forget();
        }
    }

    private async UniTask StartSpiderFight()
    {
        _wasTriggered = true;
        await CloseDoor();
        await TriggerSpiders();
        await WaitUntilAllSpidersAreDead();
        await OpenDoor();
    }

    private async UniTask CloseDoor()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await _door.DOLocalMoveY(-3.5f, 0.1f).SetRelative().WithCancellation(ct);
    }

    private async UniTask OpenDoor()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await _door.DOLocalMoveY(3.5f, 1f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
    }

    private async UniTask TriggerSpiders()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitForSeconds(3f, cancellationToken: ct);

        foreach (var spider in _spiders)
        {
            spider.Activate();
        }
    }

    private async UniTask WaitUntilAllSpidersAreDead()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        bool spidersAreAlive = true;

        while (spidersAreAlive && !ct.IsCancellationRequested)
        {
            foreach (var spider in _spiders)
            {
                if (spider == null)
                    continue;

                if (spider.GetComponent<HealthComponent>().IsDead)
                    spidersAreAlive = false;
            }
            await UniTask.WaitForSeconds(1f, cancellationToken: ct);
        }
    }
}
