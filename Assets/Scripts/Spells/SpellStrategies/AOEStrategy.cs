using UnityEngine;

[System.Serializable]
public class AOEStrategy : SpellCastingStrategy
{
    public float length;
    public float width;
    public override void Cast(ISpells spell, Transform target, int playerInt, Transform casterTransform)
    {

    }
}
