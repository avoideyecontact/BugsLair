using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class Ending : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private Button _exitButton;

    private bool _triggered;
    private ISceneLoader _sceneLoader;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(ISceneLoader sceneLoader, IEventBus eventBus)
    {
        _sceneLoader = sceneLoader;
        _eventBus = eventBus;
        SubcribeToEventBus();
        _exitButton.onClick.AddListener(OnQuit);
    }

    private void OnDestroy()
    {
        UnsubcribeToEventBus();
    }

    private void SubcribeToEventBus()
    {
        _eventBus.Subscribe<GameResumed>(OnGameResumed);
    }

    private void UnsubcribeToEventBus()
    {
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
    }

    private void Start()
    {
        _canvas.enabled = false;
        _group.alpha = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (other.CompareTag("Player"))
        {
            ShowEndScreen().Forget();
        }
    }

    public async UniTask ShowEndScreen()
    {
        _triggered = true;
        _canvas.enabled = true;

        var ct = this.GetCancellationTokenOnDestroy();

        await _group.DOFade(1f, 2f).SetEase(Ease.Flash).WithCancellation(ct);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputReader>().enabled = false;
        ShowCursor();
    }

    private void OnGameResumed(GameResumed evt)
    {
        if (_triggered)
            ShowCursor();
    }

    private void OnQuit()
    {
        _sceneLoader.LoadSceneAsync("1_Menu", useFade: true);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
