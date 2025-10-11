using UnityEngine;
using VContainer;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private GameObject _playerGameObject;

    private CameraManager _cameraManager;
    private IPlayerDataLoader _playerDataLoader;

    [Inject]
    public void Construct(CameraManager cameraManager, IPlayerDataLoader playerDataLoader)
    {
        _cameraManager = cameraManager;
        _playerDataLoader = playerDataLoader;
    }

    public void SpawnPlayer()
    {
        var player = Instantiate(_playerGameObject);
        InitializePlayerComponents(player, _playerDataLoader.PlayerData);
        _cameraManager.SetupPlayer(player.transform);
    }

    private void InitializePlayerComponents(GameObject player, PlayerData playerData)
    {
        var health = player.GetComponent<PlayerHealth>();
        health.Initialize(playerData.health);
        var weaponary = player.GetComponent<PlayerWeaponary>();
        weaponary.Initialize(playerData);
    }
}
