using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TeslaTower : MonoBehaviour
{
    [SerializeField] private Transform _teslaCenter;
    [SerializeField] private Transform _teslaField;
    [SerializeField] private float _radius = 150;
    [SerializeField] private float _damage = 1;
    [SerializeField] private float _cooldown = 0.05f;

    private Transform _player;
    private bool _isCooldown;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        PowerPlantsCheck().Forget();
    }

    private void FixedUpdate()
    {
        if (_player == null)
            return;

        if (_isCooldown)
            return;

        if (Vector3.Distance(_player.position, _teslaCenter.position) <= _radius)
        {
            var health = _player.GetComponent<PlayerHealth>();
            health.DealDamage(_damage);
            StartCooldown().Forget();
        }
    }

    private async UniTask StartCooldown()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _isCooldown = true;
        await UniTask.WaitForSeconds(_cooldown, cancellationToken: ct);
        _isCooldown = false;
    }

    private async UniTask DisableTesla()
    {
        await UniTask.WaitForSeconds(2f);
        await _teslaField.DOScale(Vector3.zero, 5f).SetEase(Ease.InElastic);
        _teslaField.gameObject.SetActive(false);
        enabled = false;
    }

    private async UniTask PowerPlantsCheck()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        while (!ct.IsCancellationRequested && enabled)
        {
            await UniTask.WaitForSeconds(2f, cancellationToken: ct);

            bool powerPlantsActive = false;
            var plants = GameObject.FindGameObjectsWithTag("PowerPlant");
            Debug.Log(plants.Length);
            foreach (var p in plants)
            {
                if (p.GetComponent<PowerPlant>().IsRunning)
                {
                    powerPlantsActive = true;
                    break;
                }
            }

            if (!powerPlantsActive)
            {
                DisableTesla().Forget();
                Debug.Log("Отключение катушек теслы");
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_teslaCenter == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_teslaCenter.position, _radius);
    }
}
