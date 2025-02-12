using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFactory : MonoBehaviour
{
    protected GameObject[] prefabs;
    protected ObjectPool<Enemy> pool = new ObjectPool<Enemy>();
    [SerializeField] private List<Enemy> startingEnemies;
    [SerializeField] private Player player;

    protected Enemy strongest;
    protected Enemy weakest;

    private void Start()
    {
        foreach (Enemy enemy in startingEnemies)
        {
            enemy.OnDeath += (float experience) => pool.Register(enemy);
            enemy.OnDeath += (float experience) => player.experience.AddXP(experience);
        }
    }
    public Enemy SpawnRandomEnemy()
    {
        if (pool.Count > 0)
        {
            Enemy inactiveEnemy = pool.FindInactive();
            inactiveEnemy.gameObject.SetActive(true);
            return inactiveEnemy;
        }

        GameObject instance = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        Enemy enemy = instance.GetComponent<Enemy>();
        enemy.OnDeath += (float experience) => pool.Register(enemy);
        enemy.OnDeath += (float experience) => player.experience.AddXP(experience);
        return enemy;
    }

    public Enemy SpawnStrongEnemy()
    {
        FindStrongest();

        if (pool.Count > 0)
        {
            Enemy inactiveEnemy = pool.SearchFor(EnemyIsStrongest);

            if (inactiveEnemy != null)
            {
                inactiveEnemy.gameObject.SetActive(true);
                return inactiveEnemy;
            }
        }

        GameObject instance = Instantiate(strongest.gameObject);
        Enemy enemy = instance.GetComponent<Enemy>();
        enemy.OnDeath += (float experience) => pool.Register(enemy);
        enemy.OnDeath += (float experience) => player.experience.AddXP(experience);

        return enemy;
    }

    public Enemy SpawnWeakEnemy()
    {
        FindWeakest();

        if (pool.Count > 0)
        {
            Enemy inactiveEnemy = pool.SearchFor(EnemyIsWeakest);

            if (inactiveEnemy != null)
            {
                inactiveEnemy.gameObject.SetActive(true);
                return inactiveEnemy;
            }
        }

        GameObject instance = Instantiate(weakest.gameObject);
        Enemy enemy = instance.GetComponent<Enemy>();
        enemy.OnDeath += (float experience) => pool.Register(enemy);
        enemy.OnDeath += (float experience) => player.experience.AddXP(experience);

        return enemy;
    }

    protected abstract void FindStrongest();
    protected abstract void FindWeakest();
    public abstract bool EnemyIsStrongest(Enemy enemy);
    public abstract bool EnemyIsWeakest(Enemy enemy);
}
