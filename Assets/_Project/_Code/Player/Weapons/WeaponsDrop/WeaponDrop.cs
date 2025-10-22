using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    public WeaponType GetWeaponType => _weaponType;
}
