using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _structures;

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
        Instantiate(_structures[randomIndex], spawnPoint);
    }
}
