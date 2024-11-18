using UnityEngine;

[System.Serializable]
public abstract class SpellCastingStrategy
{
    public abstract void Cast(ISpells spell, Vector3 offset, Transform target, int playerInt);
}
