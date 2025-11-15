using UnityEngine;

public class RandomDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] _dropGameObjects;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _probability = 1;

    public void CreateDrop()
    {
        if (_dropGameObjects == null || _dropGameObjects.Length == 0)
            return;

        double randomValue = Random.value;

        if (randomValue <= _probability)
        {
            int randomId = Random.Range(0, _dropGameObjects.Length);
            Instantiate(_dropGameObjects[randomId], transform.position + _offset, Quaternion.identity);
        }
        else
        {
            return;
        }
    }
}
