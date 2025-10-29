using UnityEngine;
using VContainer;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _abilitiesGameObjects;

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
        //ActivateAbility(AbilityType.JumpModule);
        ActivateAbility(AbilityType.Dash);
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
}
