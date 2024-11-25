using UnityEngine;

[System.Serializable]
public class AOEStrategy : SpellCastingStrategy
{
    public float length;
    public float width;
    public override void Cast(ISpells spell, Vector3 offSet, Transform target, int playerInt)
    {

    }
}
