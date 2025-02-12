using System.Collections;
using UnityEngine;

[System.Serializable]
public class MultipleStrategy : SpellCastingStrategy
{
    public override void Cast(ISpells spell, Transform target, int playerInt, Transform casterTransform)
    {
        ServiceLocator.instance.GetService<CoroutineRunner>(typeof(CoroutineRunner)).StartRoutine(CastWithDelay(spell, target, playerInt, casterTransform));
    }

    private IEnumerator CastWithDelay(ISpells spell, Transform target, int playerInt, Transform casterTransform)
    {
        for (int i = 0; i < spell.Level; i++)
        {
            SpellController controller = SpellController.Instantiation(spell, casterTransform);

            controller._target = target;
            controller._playerInt = playerInt;
            controller._currentSpell = spell;

            yield return new WaitForSeconds(0.2f);
        }
    }


}
