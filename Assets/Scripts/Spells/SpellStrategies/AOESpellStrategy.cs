using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AoESpellStrategy", menuName = "SpellCastingStrategy/AoE")]
public class AoESpellStrategy : SpellCastingStrategy
{
    [SerializeField] float radius;
    public override void CastSpell(Spells spell, Vector3 playerPos, Transform target)
    {

    }
}
