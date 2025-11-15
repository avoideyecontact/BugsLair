using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private int _howMuchCanSpawn = 30;
    [SerializeField] private GameObject _cockroach;
    [SerializeField] private GameObject _spider;
    [SerializeField] private float _triggerRadius = 150;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform[] _spawnPoints;

    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        Behaviour().Forget();
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(_player.position, transform.position);
    }

    private int CockroachesCount()
    {
        return GameObject.FindObjectsByType<CockroachAI>(FindObjectsSortMode.None).Length;
    }

    private async UniTask Behaviour()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        while (!ct.IsCancellationRequested)
        {
            if (DistanceToPlayer() < _triggerRadius)
            {
                var count = CockroachesCount();

                if (count < 100)
                {
                    SpawnCockroaches();
                }
            }

            await UniTask.WaitForSeconds(2f, cancellationToken: ct);
        }
    }

    private void SpawnCockroaches()
    {
        //if (_howMuchCanSpawn <= 0)
        //{
        //    Destroy(this);
        //    return;
        //}

        foreach (var point in _spawnPoints)
        {
            Instantiate(_cockroach, point.position, Quaternion.identity);
            Instantiate(_cockroach, point.position, Quaternion.identity);
            //_howMuchCanSpawn -= 1;
        }
    }
}
