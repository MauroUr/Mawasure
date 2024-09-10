using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellController : MonoBehaviour
{
    public Spells _currentSpell;
    public Transform _target;
    public int _playerInt;

    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, _target.position, 0.1f);

        Vector3 direction = _target.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        this.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
    }
    public static void Cast(Spells spell,Vector3 playerPos, Transform target, int playerInt)
    {
        SpellController.ExecuteCastingStrategy(spell, playerPos, target, playerInt);
    }

    private static void ExecuteCastingStrategy(Spells spell, Vector3 playerPos, Transform target, int playerInt)
    {
        SpellController controller;
        switch (spell.conditionalData.strategy)
        {
            case ConditionalData.castingStrategy.Single:
                controller = Instantiate(spell.prefab, playerPos + spell.offset, Quaternion.identity).GetComponent<SpellController>();
                controller._target = target;
                controller._playerInt = playerInt;
                controller._currentSpell = spell;
                break;
            case ConditionalData.castingStrategy.Multiple:
                controller = Instantiate(spell.prefab, playerPos + spell.offset, Quaternion.identity).GetComponent<SpellController>();
                controller._target = target;
                controller._playerInt = playerInt;
                controller._currentSpell = spell;
                FlyweightFactory.instance.SuscribeProjectile(spell.id, controller.gameObject.GetComponent<MeshRenderer>());
                controller.MultiCast(playerPos);
                break;
            case ConditionalData.castingStrategy.AOE:
                
                break;
            case ConditionalData.castingStrategy.Chains:
                
                break;
        }
    }

    public void MultiCast(Vector3 playerPos)
    {
        StartCoroutine(Multicast(playerPos));
    }
    private IEnumerator Multicast(Vector3 playerPos)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < _currentSpell.level-1; i++)
        {
            ProjectileFlyweight flyweight = FlyweightFactory.instance.GetProjectile(_currentSpell.id);
            SpellController controller = _currentSpell.prefab.gameObject.GetComponent<SpellController>();
            controller._target = this._target;
            controller._playerInt = this._playerInt;
            controller._currentSpell = this._currentSpell;

            MeshRenderer mesh = Instantiate(_currentSpell.prefab, playerPos + _currentSpell.offset, Quaternion.identity).GetComponent<MeshRenderer>();
            mesh.material = flyweight.material;
            mesh.bounds.size.Set(flyweight.size.x, flyweight.size.y, flyweight.size.z);

            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_currentSpell.conditionalData.strategy is ConditionalData.castingStrategy.Single)
            other.gameObject.GetComponent<Character>().TakeDamage(_currentSpell.dmgPerLevel * _currentSpell.level * _playerInt);
        else
            other.gameObject.GetComponent<Character>().TakeDamage(_currentSpell.dmgPerLevel);
        Destroy(this.gameObject);
    }
}
