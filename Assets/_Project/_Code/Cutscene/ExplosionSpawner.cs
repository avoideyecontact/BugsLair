using Cysharp.Threading.Tasks;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private Vector3 _zoneSize = Vector3.one;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem[] _particlePrefabs;

    [Range(1f, 10f)]
    [SerializeField] private float _scale;
    [Range(1f, 5f)]
    [SerializeField] private float _scaleDelta;

    [SerializeField] private Transform _parent;

    void Start()
    {
        Spawn().Forget();
        //Spawn().Forget();
    }

    private async UniTask Spawn()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        while (!ct.IsCancellationRequested)
        {
            if (gameObject.activeSelf)
            {
                Vector3 randomPosition = transform.position + new Vector3(
                    Random.Range(-_zoneSize.x / 2, _zoneSize.x / 2),
                    Random.Range(-_zoneSize.y / 2, _zoneSize.y / 2),
                    Random.Range(-_zoneSize.z / 2, _zoneSize.z / 2)
                );

                var newParticleSystem = Instantiate(
                    _particlePrefabs[Random.Range(0, _particlePrefabs.Length)],
                    randomPosition,
                    Quaternion.identity,
                    _parent
                );

                var scale = Vector3.one * Random.Range(_scale - _scaleDelta, _scale + _scaleDelta);
                newParticleSystem.transform.localScale = scale;

                Destroy(newParticleSystem.gameObject, 3);
            }

            var rand = Random.value;
            await UniTask.WaitForSeconds(rand, cancellationToken: ct);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, _zoneSize);
    }
}
