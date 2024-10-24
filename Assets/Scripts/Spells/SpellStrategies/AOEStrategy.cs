using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AOEStrategy : SpellCastingStrategy
{
    public float length;
    public float width;
    public override void Cast(Spells spell, Vector3 playerPos, Transform target, int playerInt)
    {

    }
}
