using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _health;

    public float Health => _health;

    public void Initialize(float health)
    {
        SetHealth(health);
    }

    public void AddHealth(float value)
    {
        _health += value;
    }

    public void SetHealth(float value)
    {
        _health = value;
    }
}
