using UnityEngine;

[System.Serializable]
public class ChainStrategy : SpellCastingStrategy
{
    public int maxEnemiesAffected;
    public float rangeBetweenEnemies;
    public override void Cast(ISpells spell, Vector3 offSet, Transform target, int playerInt)
    {

    }
}
