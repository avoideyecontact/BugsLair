using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class WeaponMinigun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponConfig _config;
    [SerializeField] private ParticleSystem _muzzleFlash1;
    [SerializeField] private ParticleSystem _muzzleFlash2;
    [SerializeField] private SoundData _fireSound;
    [SerializeField] private SoundData _emptySound;

    public WeaponType WeaponType => _config.weaponType;
    public float Damage => _config.damage;
    public float DamageRate => _config.damageRate;
    public bool Available { get; set; }
    public bool Selected { get; set; }
    public int Ammo => _ammoSystem.CurrentAmmo;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;
    private AmmoSystem _ammoSystem;
    private bool _isCooldown;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
        _ammoSystem = new AmmoSystem(_config.maxAmmo);
    }

    public void Use()
    {
        if (_isCooldown)
            return;

        if (!_ammoSystem.HasAmmo)
        {
            SoundManager.Instance.CreateSoundBuilder()
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play(_emptySound);

            WeaponCooldownTimer().Forget();
            return;
        }

        _ammoSystem.SpendAmmo();
        OnAmmoChanged();
        DealDamage();
        Recoil().Forget();
        _muzzleFlash1?.Play();
        _muzzleFlash2?.Play();

        SoundManager.Instance.CreateSoundBuilder()
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play(_fireSound);

        WeaponCooldownTimer().Forget();
    }

    private void DealDamage()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _config.hitDistance, _config.enemyLayer))
        {
            hit.transform.GetComponent<HealthComponent>()?.DealDamage(_config.damage);
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
        _ammoSystem.AddAmmo(value);
        OnAmmoChanged();
    }

    private async UniTask Recoil()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var pushForce = 3f;
        var pushDuration = 0.1f;
        Vector3 pushVelocity = Vector3.zero;

        Vector3 pushDirection = -_playerContext.MainCamera.transform.forward;

        pushDirection.y = 0;
        pushDirection.Normalize();

        pushVelocity = pushDirection * pushForce;
        var pushTimeRemaining = pushDuration;

        while (pushTimeRemaining > 0 && !ct.IsCancellationRequested)
        {
            _playerContext.Controller.Move(pushVelocity * Time.deltaTime);

            pushTimeRemaining -= Time.deltaTime;
            pushVelocity = Vector3.Lerp(pushVelocity, Vector3.zero, Time.deltaTime * 2f);
            await UniTask.Yield();
        }
    }

    private void OnAmmoChanged()
    {
        _eventBus.Publish(new AmmoChanged
        {
            Ammo = _ammoSystem.CurrentAmmo,
            WeaponType = WeaponType
        });
    }
}
