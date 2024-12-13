using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<EnemyFactory> allFactories;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] private List<GameObject> floorList = new List<GameObject>();
    private List<Transform> _floorSquares = new List<Transform>();
    private void Start()
    {
        StartCoroutine(GetEnemyFactories());
    }

    private IEnumerator GetEnemyFactories()
    {
        while (WindowsManager.instance == null)
            yield return null;

        EnemyFactoryService factoriesService = null;
        while (factoriesService == null)
        {
            factoriesService = ServiceLocator.instance.GetService<EnemyFactoryService>(typeof(EnemyFactoryService));
            yield return null;
        }
        allFactories = factoriesService.GetFactories();
    }

    public void Spawn()
    {
        if (Random.Range(0, 2) == 1)
            allFactories[Random.Range(0, allFactories.Count)].SpawnStrongEnemy();
        else
            allFactories[Random.Range(0, allFactories.Count)].SpawnWeakEnemy();

        SpawnRandomEnemies(enemiesToSpawn);
    }
    private void SpawnRandomEnemies(int quantity)
    {
        for (int i = 0;i< quantity;i++)
        {
            Enemy newEnemy = allFactories[Random.Range(0, allFactories.Count)].SpawnRandomEnemy();
            Relocate(newEnemy);
        }
    }

    private void Relocate(Enemy newEnemy)
    {
        MeshRenderer selectedRenderer = floorList[Random.Range(0, floorList.Count)].GetComponentInChildren<MeshRenderer>();

        if (selectedRenderer == null)
        {
            Debug.LogError("Floor object missing!");
            return;
        }

        Bounds bounds = selectedRenderer.bounds;
        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0.6f,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            newEnemy.transform.position = hit.position;
        else
            Debug.LogWarning("Failed to relocate enemy to a valid NavMesh position!");
    }

}

