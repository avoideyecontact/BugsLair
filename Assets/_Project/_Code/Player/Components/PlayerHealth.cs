using UnityEngine;
using VContainer;

// rewrite with inheritance
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 100;

    public float GetHealth => _health;
    public bool CanHeal => _health < _maxHealth;
    public bool IsDead => _health <= 0;

    private IEventBus _eventBus;

    [Inject]
    public void Construct(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void Initialize(float health)
    {
        SetHealth(health);
    }

    public void Heal(float value)
    {
        SetHealth(_health + value);
    }

    public void DealDamage(float value)
    {
        SetHealth(_health - value);

        _eventBus.Publish(new PlayerDamaged
        {
            DamageValue = value
        });
    }

    public void SetHealth(float value)
    {
        if (value > _maxHealth)
            _health = _maxHealth;
        else
            _health = value;

        _eventBus.Publish(new PlayerHealthChanged
        {
            HealthValue = _health
        });

        if (IsDead)
            _eventBus.Publish(new PlayerDeath{});
    }
}
