using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] WeaponType _ammoType;
    [SerializeField] int value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<PlayerWeaponary>().AddAmmo(_ammoType, value);
            Destroy(gameObject);
        }
    }
}
