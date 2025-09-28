using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private ISceneLoader _sceneLoader;

    private void Start()
    {
        if (_playButton == null)
            _playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        if (_settingsButton == null)
            _settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();

        if (_quitButton == null)
            _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

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
        _sceneLoader.LoadSceneAsync("2_Cutscene");
    }

    private void OnSettingsButtonClicked()
    {

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
