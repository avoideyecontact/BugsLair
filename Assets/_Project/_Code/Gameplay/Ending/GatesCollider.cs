using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class GatesCollider : MonoBehaviour
{
    [SerializeField] private Transform _gatesTransform;
    [SerializeField] private AudioSource _audio;
    private bool _triggered;

    private void Start()
    {
        //OpenGates().Forget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (other.CompareTag("Player"))
        {
            _triggered = true;
            OpenGates().Forget();
        }
    }

    private async UniTask OpenGates()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _audio.Play();
        await _gatesTransform.DOLocalMoveY(13f, 5f).SetEase(Ease.InOutSine).WithCancellation(ct);
    }
}
