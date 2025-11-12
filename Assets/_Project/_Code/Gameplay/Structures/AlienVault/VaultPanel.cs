using UnityEngine;

public class VaultPanel : MonoBehaviour
{
    [SerializeField] private AlienVault _vault;

    public void Open()
    {
        _vault.OpenVaultDoor();
        Destroy(this);
    }
}
