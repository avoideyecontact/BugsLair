using UnityEngine;
using VContainer;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _abilitiesGameObjects;
    [SerializeField] private LayerMask _dropLayer;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
    }

    private void Start()
    {
        SubscribeToInput();
        //ActivateAbility(AbilityType.JumpModule);
        //ActivateAbility(AbilityType.Dash);
    }

    private void OnDestroy() => UnsubscribeFromInput();

    private void SubscribeToInput()
    {
        _playerContext.Input.InteractStarted += PickupAbility;
    }

    private void UnsubscribeFromInput()
    {
        _playerContext.Input.InteractStarted -= PickupAbility;
    }

    private void ActivateAbility(AbilityType abilityType)
    {
        foreach (var abilityGameObject in _abilitiesGameObjects)
        {
            var ability = abilityGameObject.GetComponent<IAbility>();
            if (ability.AbilityType == abilityType)
            {
                ability.Activate();
            }
        }
    }

    private void DeactivateAbility(AbilityType abilityType)
    {
        foreach (var abilityGameObject in _abilitiesGameObjects)
        {
            var ability = abilityGameObject.GetComponent<IAbility>();
            if (ability.AbilityType == abilityType)
            {
                ability.Deactivate();
            }
        }
    }

    public void PickupAbility()
    {
        AbilityType? abilityType = null;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _playerContext.InteractionDistance, _dropLayer))
        {
            abilityType = hit.transform.GetComponent<AbilityDrop>()?.GetAbilityType;
        }

        if (abilityType == null)
            return;

        Destroy(hit.transform.gameObject);

        ActivateAbility((AbilityType)abilityType);
    }
}
