using UnityEngine;

[System.Serializable]
public class WeaponConfig
{
    public WeaponType weaponType;
    public float damage = 1f;
    public float damageRate = 0.1f;
    public float hitDistance = 100f;
    public LayerMask enemyLayer;
    public int maxAmmo = 50;
}
