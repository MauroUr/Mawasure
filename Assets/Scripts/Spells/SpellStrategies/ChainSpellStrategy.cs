using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChainSpellStrategy", menuName = "SpellCastingStrategy/Chain")]
public class ChainSpellStrategy : SpellCastingStrategy
{
    [SerializeField] int maxEnemiesAffected;
    [SerializeField] float maxRange;
    public override void CastSpell(Spells spell, Vector3 playerPos, Transform target)
    {

    }
}
