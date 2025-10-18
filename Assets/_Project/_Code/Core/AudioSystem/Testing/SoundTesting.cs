using System.Collections;
using UnityEngine;

public class SoundTesting : MonoBehaviour
{
    [SerializeField] private SoundData _soundData;
    private bool _canShoot = true;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _canShoot)
        {
            SoundManager.Instance.CreateSoundBuilder()
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play(_soundData);

            _canShoot = false;
            StartCoroutine(AudioCooldown());
        }
    }

    IEnumerator AudioCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        _canShoot = true;
    }
}
