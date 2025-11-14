using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class WeaponSaw : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponConfig _config;
    [SerializeField] private AudioSource _chainsawSound;

    public WeaponType WeaponType => _config.weaponType;
    public float Damage => _config.damage;
    public float DamageRate => _config.damageRate;
    public int Ammo => -1;
    public bool Available { get; set; }
    public bool Selected { get; set; }

    private BoxCollider _damageCollider;
    private bool _isCooldown;
    private PlayerContext _playerContext;
    private float _initialVolume;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    // change in the future
    private void FixedUpdate()
    {
        if (_playerContext.Input.attack > 0)
        {
            if (!_chainsawSound.isPlaying)
                _chainsawSound.Play();
            if (_chainsawSound.volume < _initialVolume)
                _chainsawSound.volume += _initialVolume * Time.deltaTime * 5;
        }
        else
        {
            if (_chainsawSound.volume > 0)
                _chainsawSound.volume -= _initialVolume * Time.deltaTime * 2.5f;
        }
    }

    private void OnDisable()
    {
        _chainsawSound?.Stop();
    }

    private void Start()
    {
        _initialVolume = _chainsawSound.volume;
        _damageCollider = GetComponent<BoxCollider>();
    }

    public void Use()
    {
        if (_isCooldown)
            return;

        DealDamage();

        WeaponCooldownTimer().Forget();
    }

    private void DealDamage()
    {
        var hits = Physics.OverlapBox(
            _damageCollider.transform.position,
            _damageCollider.size,
            _damageCollider.transform.rotation,
            _config.enemyLayer);

        foreach (var hit in hits)
        {
            hit.GetComponent<HealthComponent>()?.DealDamage(_config.damage);
        }
    }

    private async UniTask WeaponCooldownTimer()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _isCooldown = true;
        await UniTask.WaitForSeconds(_config.damageRate, cancellationToken: ct);
        _isCooldown = false;
    }

    public void AddAmmo(int value)
    {
        Debug.LogWarning("Saw doesnt use ammo");
    }

    void OnDrawGizmos()
    {
        var damageCollider = GetComponent<BoxCollider>();

        Gizmos.color = Color.red;
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = damageCollider.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(damageCollider.center, damageCollider.size);
    }
}
