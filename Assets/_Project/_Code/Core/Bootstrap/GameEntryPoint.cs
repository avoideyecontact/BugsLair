using UnityEngine.SceneManagement;
using VContainer.Unity;

public class GameEntryPoint : IStartable
{
    private readonly ISceneLoader _sceneLoader;

    public GameEntryPoint(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    void IStartable.Start()
    {
        if (SceneManager.GetActiveScene().name == "0_Bootstrap")
        {
            _sceneLoader.LoadSceneAsync("1_Menu");
        }
    }
}
