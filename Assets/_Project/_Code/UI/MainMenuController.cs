using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas _settingsCanvas;

    private ISceneLoader _sceneLoader;

    private void Start()
    {
        _mainCanvas.enabled = true;
        _settingsCanvas.enabled = false;

        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void OnPlayButtonClicked()
    {
        _sceneLoader.LoadSceneAsync("2_Cutscene", useFade: true);
    }

    private void OnSettingsButtonClicked()
    {
        _mainCanvas.enabled = false;
        _settingsCanvas.enabled = true;
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
