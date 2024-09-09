using System;
using System.Collections;
using UnityEngine;

public abstract class SpellCastingStrategy : ScriptableObject
{
    public abstract void CastSpell(Spells spell, Vector3 playerPos, Transform target);

}




