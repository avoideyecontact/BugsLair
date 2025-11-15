using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Airdrop : MonoBehaviour
{
    [SerializeField] public AirDropType _airdropType;

    [SerializeField] private Transform _door1;
    [SerializeField] private Transform _door2;
    [SerializeField] private Transform _door3;

    [SerializeField] private GameObject _minigunDrop;
    [SerializeField] private GameObject _minigunAmmoDrop;
    [SerializeField] private GameObject _lasergunAmmoDrop;
    [SerializeField] private GameObject _healthDrop;

    [SerializeField] private Transform[] _spawnPoints; 

    private void Start()
    {
        Land().Forget();
    }

    private async UniTask Land()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit);
        Debug.Log(hit.point);
        await transform.DOMoveY(hit.point.y, 10f).SetEase(Ease.OutQuart).WithCancellation(ct);
        await OpenDoors();
        SpawnDrop();
        await UniTask.WaitForSeconds(20f, cancellationToken: ct);
        await CloseDoors();
        await UniTask.WaitForSeconds(1f, cancellationToken: ct);
        await FlyAway();
        Destroy(gameObject, 1);
    }

    private async UniTask FlyAway()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        await transform.DOLocalMoveY(1000, 15f).SetEase(Ease.InSine).WithCancellation(ct);
    }

    private async UniTask OpenDoors()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var open1 = _door1.DOLocalRotate(new Vector3(-120, 0, 0), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine).WithCancellation(ct);
        var open2 = _door2.DOLocalRotate(new Vector3(-120, 0, 0), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine).WithCancellation(ct);
        var open3 = _door3.DOLocalRotate(new Vector3(-120, 0, 0), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine).WithCancellation(ct);

        await UniTask.WhenAll(open1, open2, open3);
    }

    private async UniTask CloseDoors()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var close1 = _door1.DOLocalRotate(new Vector3(120, 0, 0), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine).WithCancellation(ct);
        var close2 = _door2.DOLocalRotate(new Vector3(120, 0, 0), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine).WithCancellation(ct);
        var close3 = _door3.DOLocalRotate(new Vector3(120, 0, 0), 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine).WithCancellation(ct);

        await UniTask.WhenAll(close1, close2, close3);
    }

    private void SpawnDrop()
    {
        switch (_airdropType)
        {
            case AirDropType.Random:
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        SpawnAmmo();
                        break;
                    case 1:
                        SpawnMinigun();
                        break;
                    case 2:
                        SpawnHealth();
                        break;
                    default:
                        break;
                }
                break;
            case AirDropType.Ammo:
                SpawnAmmo();
                break;
            case AirDropType.Minigun:
                SpawnMinigun();
                break;
            case AirDropType.Health:
                SpawnHealth();
                break;
            default:
                break;
        }
    }

    private void SpawnAmmo()
    {
        foreach (var point in _spawnPoints)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
                Instantiate(_minigunAmmoDrop, point.position, Quaternion.identity);
            if (rand == 1)
                Instantiate(_lasergunAmmoDrop, point.position, Quaternion.identity);
        }
    }
    private void SpawnHealth()
    {
        foreach (var point in _spawnPoints)
        {
            Instantiate(_healthDrop, point.position, Quaternion.identity);
        }
    }

    private void SpawnMinigun()
    {
        Instantiate(_minigunDrop, transform.position + Vector3.up * 2, Quaternion.identity);

        foreach (var point in _spawnPoints)
        {
            Instantiate(_minigunAmmoDrop, point.position, Quaternion.identity);
        }
    }
}
