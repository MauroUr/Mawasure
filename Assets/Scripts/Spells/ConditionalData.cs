using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionalData
{
    [SerializeReference] public SpellCastingStrategy strategy;

    public int maxEnemiesAffected;
    public float rangeBetweenEnemies;

    public float length;
    public float width;
}

