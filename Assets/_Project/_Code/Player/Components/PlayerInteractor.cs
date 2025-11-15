using UnityEngine;
using VContainer;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] LayerMask _dropLayer;
    [SerializeField] float _interactionDistance;
    [SerializeField] private ItemInventory _items;

    private PlayerContext _playerContext;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        SubscribeToInput();
    }

    private void OnDestroy()
    {
        UnsubscribeFromInput();
    }

    private void SubscribeToInput()
    {
        _playerContext.Input.InteractStarted += OnInteraction;
    }

    private void UnsubscribeFromInput()
    {
        _playerContext.Input.InteractStarted -= OnInteraction;
    }

    // make this work for weapons and abilities
    private void OnInteraction()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactionDistance, _dropLayer))
        {
            if (_items.Has(ItemDropType.KeyCard))
            {
                _items.Remove(ItemDropType.KeyCard);
                hit.collider.transform.GetComponent<VaultPanel>()?.Open();
            }
        }
    }
}
