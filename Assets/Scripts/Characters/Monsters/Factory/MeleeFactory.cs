using UnityEngine;

public class MeleeFactory : EnemyFactory
{
    private void Awake()
    {
        prefabs = Resources.LoadAll<GameObject>("Enemies/MeleeEnemies");
    }
}
