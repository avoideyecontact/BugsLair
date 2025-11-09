using UnityEngine;

public class RandomDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] _dropGameObjects;
    [SerializeField] private Vector3 _offset;

    public void CreateDrop()
    {
        int randomId = Random.Range(0, _dropGameObjects.Length);
        Instantiate(_dropGameObjects[randomId], transform.position + _offset, Quaternion.identity);
    }
}
