
using UnityEngine;

public abstract class EnemyFactory : MonoBehaviour
{
    protected GameObject[] prefabs;
    public virtual Enemy SpawnEnemy()
    {
        GameObject instance = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        return instance.GetComponent<Enemy>();
    }
}
