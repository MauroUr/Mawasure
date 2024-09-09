using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotSpellStrategy", menuName = "SpellCastingStrategy/SingleShot")]
public class SingleShotSpellStrategy : SpellCastingStrategy
{
    private Caster caster = new();
    public override void CastSpell(Spells spell, Vector3 playerPos, Transform target)
    {
        caster.Cast(spell, playerPos, target);
    }

    private class Caster : MonoBehaviour
    {
        private Transform _target;
        private GameObject _spell;
        public void Cast(Spells spell, Vector3 playerPos, Transform target)
        {
            _target = target;
            _spell = Instantiate(spell.prefab, playerPos + spell.offset, Quaternion.identity);
        }
        private void Update()
        {
            _spell.transform.position = Vector3.MoveTowards(_spell.transform.position, _target.position, 0.1f);

            Vector3 direction = _target.position - _spell.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            _spell.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
        }
    }
}
