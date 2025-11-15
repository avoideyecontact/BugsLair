using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private TMP_Text _title1;
    [SerializeField] private TMP_Text _info1;
    [SerializeField] private Button _button1;

    [SerializeField] private TMP_Text _title2;
    [SerializeField] private TMP_Text _info2;
    [SerializeField] private Button _button2;

    [SerializeField] private TMP_Text _title3;
    [SerializeField] private TMP_Text _info3;
    [SerializeField] private Button _button3;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;

        _button1.onClick.AddListener(OnDropButton1Pressed);
        _button2.onClick.AddListener(OnDropButton2Pressed);
        _button3.onClick.AddListener(OnDropButton3Pressed);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEventBus();
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<GamePaused>(OnGamePaused);
        _eventBus.Subscribe<GameResumed>(OnGameResumed);
        _eventBus.Subscribe<SettingsUIOpened>(OnSettingsUIOpened);
        _eventBus.Subscribe<SettingsUIClosed>(OnSettingsUIClosed);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<GamePaused>(OnGamePaused);
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
        _eventBus.Unsubscribe<SettingsUIOpened>(OnSettingsUIOpened);
        _eventBus.Unsubscribe<SettingsUIClosed>(OnSettingsUIClosed);
    }

    private void Start()
    {
        SubscribeToEventBus();
        CloseInventory();
        CloseFix().Forget();
    }

    private async UniTask CloseFix()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitForSeconds(0.25f);
        CloseInventory();
    }

    private void OpenInventory()
    {
        _canvas.enabled = true;
        _canvas.gameObject.SetActive(true);
    }

    private void CloseInventory()
    {
        _canvas.enabled = false;
        _canvas.gameObject.SetActive(false);
    }

    private void UpdateInventory()
    {
        var abilities = _playerContext.AbilityManager.GetAbilitiesArray;

        if (abilities == null)
            return;

        _title1.text = abilities[0] != null ? AbilityInfo.Name(abilities[0].AbilityType) : "Пусто";
        _info1.text = abilities[0] != null ? AbilityInfo.Info(abilities[0].AbilityType) : "Нет данных";

        _title2.text = abilities[1] != null ? AbilityInfo.Name(abilities[1].AbilityType) : "Пусто";
        _info2.text = abilities[1] != null ? AbilityInfo.Info(abilities[1].AbilityType) : "Нет данных";

        _title3.text = abilities[2] != null ? AbilityInfo.Name(abilities[2].AbilityType) : "Пусто";
        _info3.text = abilities[2] != null ? AbilityInfo.Info(abilities[2].AbilityType) : "Нет данных";
    }

    private void OnDropButton1Pressed()
    {
        var abilities = _playerContext.AbilityManager.GetAbilitiesArray;

        if (abilities[0] == null)
            return;

        _playerContext.AbilityManager.DeactivateAbility(abilities[0].AbilityType);

        UpdateInventory();
    }

    private void OnDropButton2Pressed()
    {
        var abilities = _playerContext.AbilityManager.GetAbilitiesArray;

        if (abilities[1] == null)
            return;

        _playerContext.AbilityManager.DeactivateAbility(abilities[1].AbilityType);

        UpdateInventory();
    }

    private void OnDropButton3Pressed()
    {
        var abilities = _playerContext.AbilityManager.GetAbilitiesArray;

        if (abilities[2] == null)
            return;

        _playerContext.AbilityManager.DeactivateAbility(abilities[2].AbilityType);

        UpdateInventory();
    }

    private void OnGamePaused(GamePaused gamePaused)
    {
        OpenInventory();
        UpdateInventory();
    }

    private void OnGameResumed(GameResumed gameResumed)
    {
        CloseInventory();
    }

    private void OnSettingsUIOpened(SettingsUIOpened evt)
    {
        CloseInventory();
    }

    private void OnSettingsUIClosed(SettingsUIClosed evt)
    {
        OpenInventory();
    }
}
