using TMPro;
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

    private bool _isOpen;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
        SubscribeToEventBus();

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
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<GamePaused>(OnGamePaused);
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
    }

    private void Start()
    {
        CloseInventory();
    }

    private void ToggleInventory()
    {
        if (_isOpen) CloseInventory();
        else OpenInventory();
    }

    private void OpenInventory()
    {
        _isOpen = true;
        _canvas.enabled = true;
        ShowCursor();
    }

    private void CloseInventory()
    {
        _isOpen = false;
        _canvas.enabled = false;
        HideCursor();
    }

    private void UpdateInventory()
    {
        var abilities = _playerContext.AbilityManager.GetAbilitiesArray;

        if (abilities == null)
            return;

        _title1.text = abilities[0] != null ? abilities[0].AbilityType.ToString() : "Пусто";
        _info1.text = abilities[0] != null ? abilities[0].AbilityType.ToString() : "Нет данных";

        _title2.text = abilities[1] != null ? abilities[1].AbilityType.ToString() : "Пусто";
        _info2.text = abilities[1] != null ? abilities[1].AbilityType.ToString() : "Нет данных";

        _title3.text = abilities[2] != null ? abilities[2].AbilityType.ToString() : "Пусто";
        _info3.text = abilities[2] != null ? abilities[2].AbilityType.ToString() : "Нет данных";
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

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}
