using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiCastSpellStrategy", menuName = "SpellCastingStrategy/MultiCast")]
public class MultiCastSpellStrategy : SpellCastingStrategy
{
    private Caster caster = new();
    public override void CastSpell(Spells spell, Vector3 playerPos, Transform target)
    {
        caster.Cast(spell, playerPos, target);
    }
    private class Caster : MonoBehaviour
    {
        public void Cast(Spells spell, Vector3 playerPos, Transform target)
        {
            StartCoroutine(Multicast(spell, playerPos, target));
        }
        private IEnumerator Multicast(Spells spell, Vector3 playerPos, Transform target)
        {
            for (int i = 0; i < spell.level; i++)
            {
                ProjectileFlyweight flyweight = ProjectileFlyweightFactory.instance.GetProjectile(spell.id);
                SpriteRenderer sprite = spell.prefab.GetComponent<SpriteRenderer>();
                sprite.material.SetTexture(flyweight.texture.name, flyweight.texture);
                sprite.color = flyweight.color;
                sprite.size = flyweight.size;
                Instantiate(spell, playerPos + spell.offset, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
