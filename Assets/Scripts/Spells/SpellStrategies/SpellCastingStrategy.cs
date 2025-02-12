using UnityEngine;

[System.Serializable]
public abstract class SpellCastingStrategy
{
    public abstract void Cast(ISpells spell, Transform target, int playerInt, Transform casterTransform);
}
