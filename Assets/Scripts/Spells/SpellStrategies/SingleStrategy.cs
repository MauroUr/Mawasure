using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleStrategy : SpellCastingStrategy
{
    public override void Cast(Spells spell, Vector3 playerPos, Transform target, int playerInt)
    {
        SpellController controller = new SpellController();
        controller = controller.Instantiation(spell, playerPos);
        controller._target = target;
        controller._playerInt = playerInt;
        controller._currentSpell = spell;
    }
}
