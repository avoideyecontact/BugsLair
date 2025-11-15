using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class UFODeath : MonoBehaviour
{
    [SerializeField] private Collider _collider1;
    [SerializeField] private Collider _collider2;
    [SerializeField] private GameObject _keyCard;

    public async UniTask Disappear()
    {

        var ct = this.GetCancellationTokenOnDestroy();
        await UniTask.WaitForSeconds(10, cancellationToken: ct);

        transform.GetComponent<Rigidbody>().isKinematic = true;
        Instantiate(_keyCard, transform.position, Quaternion.identity);
        var deathTasks = new UniTask[3];
        deathTasks[0] = transform.DOLocalMoveY(-0.65f, 3f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        deathTasks[1] = transform.DOScale(0, 3f).SetEase(Ease.InOutSine).WithCancellation(ct);
        deathTasks[2] = transform.DOShakeRotation(2f, 10f).WithCancellation(ct);

        _collider1.enabled = false;
        _collider2.enabled = false;

        await UniTask.WhenAll(deathTasks);
        Destroy(gameObject);
    }
}
