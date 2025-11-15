using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FixDragonfly : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _dragonfly;
    [SerializeField] private GameObject _dragonflyFixed;
    [SerializeField] private Ending _ending;
    [SerializeField] private int _gears = 0;
    [SerializeField] private AudioSource _audio;

    private bool _fixed;

    private void Start()
    {
        _dragonfly.SetActive(true);
        _dragonflyFixed.SetActive(false);
        UpdateUI();
    }

    public void UpdateUI()
    {
        _text.text = $"Необходимо 5 шестерёнок для починки стрекозы\n{_gears} / 5";        
    }

    public void AddGears(int gears)
    {
        if (_fixed)
            return;

        _gears += gears;

        if (_gears >= 5)
        {
            _fixed = true;
            _gears = 5;
            Fix().Forget();
        }
        UpdateUI();
    }

    private async UniTask Fix()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _dragonfly.gameObject.SetActive(false);
        _dragonflyFixed.gameObject.SetActive(true);
        GetComponent<Canvas>().enabled = false;
        _audio.Play();
        await UniTask.WaitForSeconds(5f, cancellationToken: ct);
        _ending.ShowEndScreen().Forget();
    }
}
