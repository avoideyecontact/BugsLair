using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _health = 5;

    public float Health => _health;

    public void Initialize(float health)
    {
        SetHealth(health);
    }

    public void Heal(float value)
    {
        _health += value;
    }

    public void DealDamage(float value)
    {
        _health -= value;
    }

    public void SetHealth(float value)
    {
        _health = value;
    }
}
