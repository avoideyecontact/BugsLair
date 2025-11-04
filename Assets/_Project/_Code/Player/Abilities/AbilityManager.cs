using UnityEngine;
using VContainer;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _abilitiesGameObjects;
    [SerializeField] private LayerMask _dropLayer;

    private IAbility[] _abilities;
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
        _abilities = new IAbility[3];
        SubscribeToInput();
        //ActivateAbility(AbilityType.JumpModule);
        ActivateAbility(AbilityType.Dash);
        DeactivateAbility(AbilityType.Dash);
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
        if (!HasEmptySlots() || AlredyHasThisAbility(abilityType))
            return;

        foreach (var abilityGameObject in _abilitiesGameObjects)
        {
            var ability = abilityGameObject.GetComponent<IAbility>();
            if (ability.AbilityType == abilityType)
            {

                for (int i = 0; i < _abilities.Length; i++)
                {
                    if (_abilities[i] == null)
                    {
                        _abilities[i] = ability;
                        ability.Activate();
                        return;
                    }
                }

                Debug.LogError("No available slots for ability");
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

                for (int i = 0; i < _abilities.Length; i++)
                {
                    if (_abilities[i].AbilityType == abilityType)
                    {
                        _abilities[i] = null;
                        ability.Deactivate();
                        return;
                    }
                }

                Debug.LogError("Ability is missing");
            }
        }
    }

    public void PickupAbility()
    {
        if (!HasEmptySlots())
            return;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _playerContext.InteractionDistance, _dropLayer))
        {
            var abilityDrop = hit.transform.GetComponent<AbilityDrop>();

            if (abilityDrop == null)
                return;

            AbilityType abilityType = abilityDrop.GetAbilityType;

            if (AlredyHasThisAbility(abilityType))
                return;

            Destroy(hit.transform.gameObject);
            ActivateAbility(abilityType);
        }
    }

    private bool HasEmptySlots()
    {
        foreach (var ability in _abilities)
        {
            if (ability == null)
                return true;
        }
        return false;
    }

    private bool AlredyHasThisAbility(AbilityType abilityType)
    {
        foreach(var ability in _abilities)
        {
            if (ability == null)
                continue;

            if (ability.AbilityType == abilityType)
                return true;
        }
        return false;
    }
}
