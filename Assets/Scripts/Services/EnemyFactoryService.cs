using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryService : Service
{
    [SerializeField] private List<EnemyFactory> enemyFactories;

    public List<EnemyFactory> GetFactories() { return enemyFactories; }
}
