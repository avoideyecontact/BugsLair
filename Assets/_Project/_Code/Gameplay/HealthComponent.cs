using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _health = 5;

    public float Health => _health;
    public bool IsDead => _health <= 0;
    public event System.Action OnDeath;

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
    }

    public void SetHealth(float value)
    {
        _health = value;
        if (IsDead)
            OnDeath?.Invoke();
    }
}
