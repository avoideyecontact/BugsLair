using UnityEngine;
using VContainer.Unity;

public class CutsceneEntryPoint : IStartable
{
    void IStartable.Start()
    {
        Debug.Log("Cutscene!");
    }
}
