using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _structures;
    [SerializeField] private bool _randomRotation;

    private void Start()
    {
        var spawnPoints = GetComponentsInChildren<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == transform)
                continue;

            SpawnRandomStructure(spawnPoint);
        }
    }

    private void SpawnRandomStructure(Transform spawnPoint)
    {
        int randomIndex = Random.Range(0, _structures.Length);
        var structure = Instantiate(_structures[randomIndex], spawnPoint);
        if (_randomRotation)
        {
            var rotation = Random.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;

            structure.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
