using UnityEngine;
using VContainer;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private GameObject _playerGameObject;

    private CameraManager _cameraManager;

    [Inject]
    public void Construct(CameraManager cameraManager)
    {
        _cameraManager = cameraManager;
    }

    public void SpawnPlayer()
    {
        var player = Instantiate(_playerGameObject);
        _cameraManager.SetupPlayerCamera(player.transform.Find("CameraPos"));
    }
}
