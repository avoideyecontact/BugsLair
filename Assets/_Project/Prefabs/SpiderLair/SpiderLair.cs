using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class SpiderLair : MonoBehaviour
{
    [SerializeField] private Transform _door;
    [SerializeField] private Spider[] _spiders;

    [SerializeField] private AudioSource _doorSound;
    [SerializeField] private AudioSource _horrorSound;

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
        await UniTask.WaitForSeconds(1f);
        _horrorSound.Play();
        await TriggerSpiders();
        await WaitUntilAllSpidersAreDead();
        await UniTask.WaitForSeconds(1f);
        await OpenDoor();
    }

    private async UniTask CloseDoor()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await _door.DOLocalMoveY(-3.5f, 0.1f).SetRelative().WithCancellation(ct);
        _doorSound.Play();
    }

    private async UniTask OpenDoor()
    {
        _doorSound.Play();

        var ct = this.GetCancellationTokenOnDestroy();

        await _door.DOLocalMoveY(3.5f, 1f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
    }

    private async UniTask TriggerSpiders()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitForSeconds(5f, cancellationToken: ct);

        foreach (var spider in _spiders)
        {
            spider?.Activate();
            
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

                if (!spider.GetComponent<HealthComponent>().IsDead)
                    break;

                spidersAreAlive = false;
            }
            await UniTask.WaitForSeconds(1f, cancellationToken: ct);
        }
    }
}
