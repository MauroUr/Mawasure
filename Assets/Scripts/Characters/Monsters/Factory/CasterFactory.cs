
using UnityEngine;

public class CasterFactory : EnemyFactory
{
    private void Awake()
    {
        prefabs = Resources.LoadAll<GameObject>("Enemies/CasterEnemies");
    }
}
