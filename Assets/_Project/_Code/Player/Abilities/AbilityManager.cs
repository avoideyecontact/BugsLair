using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _abilitiesGameObjects;
    [SerializeField] private LayerMask _dropLayer;

    [SerializeField] private GameObject _jumpModuleAbilityDrop;
    [SerializeField] private GameObject _dashAbilityDrop;

    [SerializeField] private SoundData _pickupSound;

    private IAbility[] _abilities;
    private PlayerContext _playerContext;
    private IEventBus _eventBus;

    public IAbility[] GetAbilitiesArray => _abilities;

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

    public void DeactivateAbility(AbilityType abilityType)
    {
        foreach (var abilityGameObject in _abilitiesGameObjects)
        {
            var ability = abilityGameObject.GetComponent<IAbility>();
            if (ability.AbilityType == abilityType)
            {

                for (int i = 0; i < _abilities.Length; i++)
                {
                    if (_abilities[i] == null)
                        continue;

                    if (_abilities[i].AbilityType == abilityType)
                    {
                        ability.Deactivate();
                        _abilities[i] = null;
                        SpawnAbility(abilityType);
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

            SoundManager.Instance.CreateSoundBuilder()
                    .WithRandomPitch()
                    .WithPosition(transform.position)
                    .Play(_pickupSound);
        }
    }

    private void SpawnAbility(AbilityType abilityType)
    {
        var position = transform.root.position + transform.root.forward * 2 + transform.root.up;

        //RaycastHit hit;
        //if (Physics.Raycast(position + Vector3.up * 6, Vector3.down, out hit, 12, LayerMask.NameToLayer("Default")))
        //{
        //    position = hit.point;
        //}

        switch (abilityType)
        {
            case AbilityType.JumpModule:
                Instantiate(_jumpModuleAbilityDrop, position, Quaternion.identity);
                break;
            case AbilityType.Dash:
                Instantiate(_dashAbilityDrop, position, Quaternion.identity);
                break;
            default:
                break;
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
