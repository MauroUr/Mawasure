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
        this.transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
    }
    public static void Cast(ISpells spell,Vector3 playerPos, Transform target, int playerInt)
    {
        spell.ConditionalData.strategy.Cast(spell, playerPos, target, playerInt);
    }

    public static SpellController Instantiation(ISpells spell, Vector3 playerPos)
    {
        return Instantiate(spell.Prefab, playerPos + spell.Offset, Quaternion.identity).GetComponent<SpellController>();
    }
    public void MultiCast(Vector3 playerPos)
    {
        StartCoroutine(Multicast(playerPos));
    }
    private IEnumerator Multicast(Vector3 playerPos)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < _currentSpell.Level - 1; i++)
        {
            ProjectileFlyweight flyweight = FlyweightFactory.instance.GetProjectile(_currentSpell.Id);

            GameObject spellInstance = Instantiate(_currentSpell.Prefab, playerPos + _currentSpell.Offset, Quaternion.identity);
            SpellController controller = spellInstance.GetComponent<SpellController>();

            controller._currentSpell = _currentSpell;
            controller._target = _target;
            controller._playerInt = _playerInt;

            MeshRenderer mesh = spellInstance.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.material = flyweight.material;
                mesh.bounds.size.Set(flyweight.size.x, flyweight.size.y, flyweight.size.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Character>(out Character enemy))
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
