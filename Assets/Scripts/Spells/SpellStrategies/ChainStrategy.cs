using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChainStrategy : SpellCastingStrategy
{
    public int maxEnemiesAffected;
    public float rangeBetweenEnemies;
    public override void Cast(ISpells spell, Vector3 playerPos, Transform target, int playerInt)
    {

    }
}
