using UnityEngine;

public class WeaponSaw : MonoBehaviour, IWeapon
{
    public void Use()
    {
        Debug.Log("Using saw");
    }
}
