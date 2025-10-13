using Cysharp.Threading.Tasks;
using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
    [SerializeField] private float _timeToHide;

    private void Start()
    {
        Hide().Forget();
    }

    private async UniTask Hide()
    {
        await UniTask.WaitForSeconds(_timeToHide);
        gameObject.SetActive(false);
    }
}
