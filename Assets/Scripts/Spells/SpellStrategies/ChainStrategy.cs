using UnityEngine;

[System.Serializable]
public class ChainStrategy : SpellCastingStrategy
{
    public int maxEnemiesAffected;
    public float rangeBetweenEnemies;
    public override void Cast(ISpells spell, Transform target, int playerInt, Transform casterTransform)
    {
        
    }
}
