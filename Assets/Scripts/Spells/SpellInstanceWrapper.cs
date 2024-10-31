using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInstanceWrapper
{
    public Spells spell;
    public int instanceLevel;

    public SpellInstanceWrapper(Spells spell, int level)
    {
        this.spell = spell;
        this.instanceLevel = level;
    }
}
