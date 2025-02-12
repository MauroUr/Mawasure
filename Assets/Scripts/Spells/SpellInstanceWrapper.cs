using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInstanceWrapper : ISpells
{
    public Spells spell;
    public int instanceLevel;

    public int Id => spell.Id;

    public int Level
    {
        get => instanceLevel;
    }

    public int DmgPerLevel => spell.DmgPerLevel;

    public int ManaPerLevel => spell.ManaPerLevel;

    public float CastDelayPerLevel => spell.CastDelayPerLevel;

    public float Range => spell.Range;

    public GameObject Prefab => spell.Prefab;

    public Vector3 Offset => spell.Offset;

    public ConditionalData ConditionalData => spell.ConditionalData;

    public SpellInstanceWrapper(Spells spell, int level)
    {
        this.spell = spell;
        this.instanceLevel = level;
    }

    
}
