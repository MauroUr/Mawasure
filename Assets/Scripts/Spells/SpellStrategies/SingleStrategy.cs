using UnityEngine;

[System.Serializable]
public class SingleStrategy : SpellCastingStrategy
{
    public override void Cast(ISpells spell, Transform target, int playerInt, Transform casterTransform)
    {
        SpellController controller = SpellController.Instantiation(spell, casterTransform);
        controller._target = target;
        controller._playerInt = playerInt;
        controller._currentSpell = spell;
    }
}
