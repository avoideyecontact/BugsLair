using UnityEngine;

public class WeaponLasergun : MonoBehaviour, IWeapon
{
    public void Use()
    {
        Debug.Log("Using lasergun");
    }
}
