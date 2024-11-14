using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AOEStrategy : SpellCastingStrategy
{
    public float length;
    public float width;
    public override void Cast(ISpells spell, Transform playerTransform, Transform target, int playerInt)
    {

    }
}
