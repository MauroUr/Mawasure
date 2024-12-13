using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesStatsService : Service
{
    private Dictionary<string,EnemiesStats> enemiesStats = new Dictionary<string, EnemiesStats>();

    public EnemiesStats CheckDuplicatedStats(string prefab, EnemiesStats stats) {
        if (enemiesStats.ContainsKey(prefab))
            return enemiesStats[prefab];
        
        enemiesStats.Add(prefab, stats);
        return stats;
    }
}
