using UnityEngine;

[System.Serializable]
public abstract class SpellCastingStrategy
{
    public abstract void Cast(ISpells spell, Vector3 playerPos, Transform target, int playerInt);
}
