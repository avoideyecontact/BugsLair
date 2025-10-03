using Cysharp.Threading.Tasks;

public interface ISceneLoader
{
    UniTask LoadSceneAsync(string sceneName, bool useFade = false);
    string CurrentScene { get; }
}
