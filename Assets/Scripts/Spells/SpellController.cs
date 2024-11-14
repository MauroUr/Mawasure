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
    public static void Cast(ISpells spell,Transform playerTransform, Transform target, int playerInt)
    {
        spell.ConditionalData.strategy.Cast(spell, playerTransform, target, playerInt);
    }

    public static SpellController Instantiation(ISpells spell, Transform playerTransform)
    {
        Vector3 calculatedOffset = playerTransform.position +
                               playerTransform.right * spell.Offset.x +    
                               playerTransform.up * spell.Offset.y +        
                               playerTransform.forward * spell.Offset.z;
        return Instantiate(spell.Prefab, calculatedOffset, Quaternion.identity).GetComponent<SpellController>();
    }
    public void MultiCast(Transform playerTransform)
    {
        StartCoroutine(Multicast(playerTransform));
    }
    private IEnumerator Multicast(Transform playerTransform)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < _currentSpell.Level - 1; i++)
        {
            ProjectileFlyweight flyweight = FlyweightFactory.instance.GetProjectile(_currentSpell.Id);
            Vector3 calculatedOffset = playerTransform.position +
                               playerTransform.right * _currentSpell.Offset.x +     // Right (5 units)
                               playerTransform.up * _currentSpell.Offset.y +        // Up (10 units)
                               playerTransform.forward * _currentSpell.Offset.z;
            GameObject spellInstance = Instantiate(_currentSpell.Prefab, calculatedOffset, Quaternion.identity);
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
            yield return new WaitForSecondsRealtime(0.2f);
        }
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
