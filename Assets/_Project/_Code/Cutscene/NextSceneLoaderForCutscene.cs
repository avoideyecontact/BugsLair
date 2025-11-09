using UnityEngine;
using VContainer;

public class NextSceneLoaderForCutscene : MonoBehaviour
{
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void LoadGameplay()
    {
        _sceneLoader.LoadSceneAsync("3_Gameplay", true);
    }    
}
