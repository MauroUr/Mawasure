using System.Collections;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public ISpells _currentSpell;
    [HideInInspector] public Transform _target;
    [HideInInspector] public int _playerInt;

    private void Update()
    {
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, _target.position, 0.1f);

        Vector3 direction = _target.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        if(rotation != Quaternion.identity)
            this.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
    }
    public static void Cast(ISpells spell,Transform casterTransform, Transform target, int playerInt)
    {
        spell.ConditionalData.strategy.Cast(spell, target, playerInt, casterTransform);
    }

    public static SpellController Instantiation(ISpells spell , Vector3 offset, Transform casterTransform)
    {
        SpellController controller = Instantiate(spell.Prefab, offset, Quaternion.identity).GetComponent<SpellController>();

        Collider spellCollider = controller.gameObject.GetComponent<Collider>();
        Collider casterCollider = casterTransform.GetComponent<Collider>();

        if (spellCollider != null && casterCollider != null)
            Physics.IgnoreCollision(spellCollider, casterCollider);

        return controller;
    }


    private void OnTriggerEnter(Collider other)
    {
        Character enemy;
        if (other.gameObject.layer == 3)
            enemy = other.GetComponentInParent<Character>();
        else if (!other.gameObject.TryGetComponent<Character>(out enemy))
        {
            Destroy(this.gameObject);
            return;
        }

        if (_currentSpell.ConditionalData.strategy is SingleStrategy)
            enemy.TakeDamage(_currentSpell.DmgPerLevel * _currentSpell.Level * _playerInt);
        else
            enemy.TakeDamage(_currentSpell.DmgPerLevel);
        Destroy(this.gameObject);
    }
}
