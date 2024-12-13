using UnityEngine;

public class MeleeFactory : EnemyFactory
{
    public override bool EnemyIsStrongest(Enemy enemy)
    {
        FindStrongest();
        return enemy.stats.damage >= strongest.stats.damage;
    }

    protected override void FindStrongest()
    {
        if (strongest == null)
        {
            foreach (GameObject prefab in prefabs)
            {
                var prefabEnemy = prefab.GetComponent<Enemy>();

                if (strongest == null)
                    strongest = prefabEnemy;

                if (prefabEnemy != null)
                {
                    if (prefabEnemy.stats.damage > strongest.stats.damage)
                        strongest = prefabEnemy;
                }
            }
        }
    }

    public override bool EnemyIsWeakest(Enemy enemy)
    {
        FindWeakest();
        return enemy.stats.damage <= weakest.stats.damage;
    }

    protected override void FindWeakest()
    {
        if (weakest == null)
        {
            foreach (GameObject prefab in prefabs)
            {
                var prefabEnemy = prefab.GetComponent<Enemy>();

                if (weakest == null)
                    weakest = prefabEnemy;

                if (prefabEnemy != null)
                {
                    if (prefabEnemy.stats.damage < weakest.stats.damage)
                        weakest = prefabEnemy;
                }
            }
        }
    }

    private void Awake()
    {
        prefabs = Resources.LoadAll<GameObject>("Enemies/MeleeEnemies");
    }
}
