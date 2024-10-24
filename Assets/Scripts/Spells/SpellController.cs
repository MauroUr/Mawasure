using System.Collections;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public Spells _currentSpell;
    public Transform _target;
    public int _playerInt;

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
        this.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
    }
    public static void Cast(Spells spell,Vector3 playerPos, Transform target, int playerInt)
    {
        spell.conditionalData.strategy.Cast(spell, playerPos, target, playerInt);
    }

    public SpellController Instantiation(Spells spell, Vector3 playerPos)
    {
        return Instantiate(spell.prefab, playerPos + spell.offset, Quaternion.identity).GetComponent<SpellController>();
    }
    public void MultiCast(Vector3 playerPos)
    {
        StartCoroutine(Multicast(playerPos));
    }
    private IEnumerator Multicast(Vector3 playerPos)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < _currentSpell.level - 1; i++)
        {
            ProjectileFlyweight flyweight = FlyweightFactory.instance.GetProjectile(_currentSpell.id);
            SpellController controller = _currentSpell.prefab.gameObject.GetComponent<SpellController>();
            controller._target = this._target;
            controller._playerInt = this._playerInt;
            controller._currentSpell = this._currentSpell;

            MeshRenderer mesh = Instantiate(_currentSpell.prefab, playerPos + _currentSpell.offset, Quaternion.identity).GetComponent<MeshRenderer>();
            mesh.material = flyweight.material;
            mesh.bounds.size.Set(flyweight.size.x, flyweight.size.y, flyweight.size.z);

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentSpell.conditionalData.strategy is SingleStrategy)
            other.gameObject.GetComponent<Character>().TakeDamage(_currentSpell.dmgPerLevel * _currentSpell.level * _playerInt);
        else
            other.gameObject.GetComponent<Character>().TakeDamage(_currentSpell.dmgPerLevel);
        Destroy(this.gameObject);
    }
}
