using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public Spells currentSpell;
    public Transform target;
    public int playerInt;

    public static void Cast(Spells spell,Vector3 playerPos, Transform target, int playerInt)
    {
        SpellController controller = spell.prefab.AddComponent<SpellController>();
        controller.target = target;
        controller.playerInt = playerInt;
        controller.currentSpell = spell;
        spell.spellCastingStrategy.CastSpell(spell, playerPos, target);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (currentSpell.spellCastingStrategy is SingleShotSpellStrategy)
            other.gameObject.GetComponent<Character>().TakeDamage(currentSpell.dmgPerLevel * currentSpell.level * playerInt);
        else
            other.gameObject.GetComponent<Character>().TakeDamage(currentSpell.dmgPerLevel);
        Destroy(this.gameObject);
    }
}
