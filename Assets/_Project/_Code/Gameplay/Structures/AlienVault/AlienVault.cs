using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AlienVault : MonoBehaviour
{
    [SerializeField] private Transform _doorTransform;
    [SerializeField] private Rigidbody _doorRigidbody;
    [SerializeField] private Transform _panelTransform;
    [SerializeField] private Transform _buttonTransform;
    [SerializeField] private TMP_Text _panelText;
    [SerializeField] private Transform _keyCardTransform;

    private bool _isOpened;

    public void OpenVaultDoor()
    {
        if (_isOpened) return;

        _isOpened = true;
        OpenDoorTask().Forget();
    }

    private async UniTask OpenDoorTask()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _keyCardTransform.gameObject.SetActive(true);
        await _keyCardTransform.DOLocalMoveY(-1.75f, 2f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        _keyCardTransform.gameObject.SetActive(false);

        var mat1 = _panelTransform.gameObject.GetComponent<Renderer>().material;
        var mat2 = _buttonTransform.gameObject.GetComponent<Renderer>().material;

        LerpEmissionColor(Color.red, Color.green, .25f, mat1).Forget();
        LerpEmissionColor(Color.red, Color.green, .25f, mat2).Forget();
        await LerpTextColor(Color.red, Color.green, .25f, _panelText);

        await UniTask.WaitForSeconds(2f);
        await _buttonTransform.DOLocalMoveZ(.05f, 0.5f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        _buttonTransform.DOLocalMoveZ(-.05f, 0.25f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct).Forget();
        _panelText.text = "Осторожно";
        LerpTextColor(Color.green, Color.red, .25f, _panelText).Forget();
        await _doorTransform.DOLocalMoveZ(-0.6f, 3f).SetEase(Ease.InOutSine).SetRelative().WithCancellation(ct);
        _doorRigidbody.isKinematic = false;
        var impulsePosition = _doorTransform.position + _doorTransform.up * 2;
        _doorRigidbody.AddForceAtPosition(-50000 * _doorTransform.forward, impulsePosition);

        Destroy(this);
    }

    async UniTask LerpEmissionColor(Color start, Color end, float duration, Material mat)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            mat?.SetColor("_EmissionColor", Color.Lerp(start, end, elapsed / duration));
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        mat.SetColor("_EmissionColor", end);
    }

    async UniTask LerpTextColor(Color start, Color end, float duration, TMP_Text text)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            text.color = Color.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }
        text.color = end;
    }
}
