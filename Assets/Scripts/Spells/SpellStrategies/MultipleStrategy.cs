using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultipleStrategy : SpellCastingStrategy
{
    public override void Cast(ISpells spell, Transform playerTransform, Transform target, int playerInt)
    {
        SpellController controller = SpellController.Instantiation(spell, playerTransform);
        controller._target = target;
        controller._playerInt = playerInt;
        controller._currentSpell = spell;
        FlyweightFactory.instance.SuscribeProjectile(spell.Id, controller.gameObject.GetComponent<MeshRenderer>());
        controller.MultiCast(playerTransform);
    }

    
}
