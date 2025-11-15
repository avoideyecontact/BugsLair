using System.Linq;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _structures;
    [SerializeField] private bool _randomRotation;
    [SerializeField] private int _randomQuantity;

    private void Start()
    {
        var spawnPoints = GetComponentsInChildren<Transform>();

        if (_randomQuantity == 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint == transform)
                    continue;

                SpawnRandomStructure(spawnPoint);
            }
        }
        else
        {
            int quantity = Mathf.Min(spawnPoints.Length, _randomQuantity);

            var structures = GetRandomElements(spawnPoints, quantity);

            foreach (Transform structure in structures)
            {
                if (structure == transform)
                    continue;

                SpawnRandomStructure(structure);
            }
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

    public static T[] GetRandomElements<T>(T[] array, int count)
    {
        if (count <= 0)
        {
            Debug.LogError("Should be more than 0");
            return null;
        }

        if (count > array.Length)
        {
            Debug.LogError("Cant be more than array length");
            return null;
        }

        System.Random random = new System.Random();

        return array.OrderBy(x => random.Next()).Take(count).ToArray();
    }
}
