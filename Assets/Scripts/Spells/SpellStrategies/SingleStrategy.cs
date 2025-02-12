using UnityEngine;

[System.Serializable]
public class SingleStrategy : SpellCastingStrategy
{
    public override void Cast(ISpells spell, Transform target, int playerInt, Transform casterTransform)
    {
        Vector3 calculatedOffset = casterTransform.position +
                               casterTransform.right * spell.Offset.x +
                               casterTransform.up * spell.Offset.y +
                               casterTransform.forward * spell.Offset.z;

        SpellController controller = SpellController.Instantiation(spell, calculatedOffset, casterTransform);
        controller._target = target;
        controller._playerInt = playerInt;
        controller._currentSpell = spell;
    }
}
