
using UnityEngine;

public class CasterFactory : EnemyFactory
{
    public override bool EnemyIsStrongest(Enemy enemy)
    {
        FindStrongest();

        return (enemy as Caster).casterInt >= (strongest as Caster).casterInt;
    }

    protected override void FindStrongest()
    {
        if (strongest == null)
        {
            foreach (GameObject prefab in prefabs)
            {
                var prefabEnemy = prefab.GetComponent<Caster>();

                if (strongest == null)
                    strongest = prefabEnemy;

                if (prefabEnemy != null)
                {
                    if (prefabEnemy.casterInt > (strongest as Caster).casterInt)
                        strongest = prefabEnemy;
                }
            }
        }
    }

    public override bool EnemyIsWeakest(Enemy enemy)
    {
        FindWeakest();

        return (enemy as Caster).casterInt <= (weakest as Caster).casterInt;
    }

    protected override void FindWeakest()
    {
        if (weakest == null)
        {
            foreach (GameObject prefab in prefabs)
            {
                var prefabEnemy = prefab.GetComponent<Caster>();

                if (weakest == null)
                    weakest = prefabEnemy;

                if (prefabEnemy != null)
                {
                    if (prefabEnemy.casterInt < (weakest as Caster).casterInt)
                        weakest = prefabEnemy;
                }
            }
        }
    }

    private void Awake()
    {
        prefabs = Resources.LoadAll<GameObject>("Enemies/CasterEnemies");
    }
}
