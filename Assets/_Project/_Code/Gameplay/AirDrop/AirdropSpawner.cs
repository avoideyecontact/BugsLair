using Cysharp.Threading.Tasks;
using UnityEngine;

public class AirdropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _airdrop;
    [SerializeField] private Transform _firstAirdrop;

    private void Start()
    {
        When20CockroachesAreDead().Forget();
    }

    private async UniTask When20CockroachesAreDead()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        while (!ct.IsCancellationRequested)
        {
            if (Statistics._cockroachesKilled == 15)
            {
                SpawnAirDropWithMinigun();
                break;
            }
            await UniTask.WaitForSeconds(1f, cancellationToken: ct);
        }
    }

    private void SpawnAirDropWithMinigun()
    {
        Debug.Log("SpawnAirDropWithMinigun");
        var airdrop = Instantiate(_airdrop, _firstAirdrop);
        airdrop.GetComponent<Airdrop>()._airdropType = AirDropType.Minigun;
    }
}
