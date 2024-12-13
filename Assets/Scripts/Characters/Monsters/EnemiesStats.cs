

using UnityEngine;

[System.Serializable]
public struct EnemiesStats
{
    [SerializeField] public float radius;
    [SerializeField] public float attackRadius;

    [SerializeField] public float damage;
    [SerializeField] public float attackDelay;

    [SerializeField] public float experience;
    [SerializeField] public bool canCast;
}
