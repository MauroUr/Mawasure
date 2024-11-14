using UnityEngine;

[System.Serializable]
public abstract class SpellCastingStrategy
{
    public abstract void Cast(ISpells spell, Transform playerTransform, Transform target, int playerInt);
}
