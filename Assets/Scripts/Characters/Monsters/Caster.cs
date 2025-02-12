using System.Collections;
using UnityEngine;

public class Caster : Enemy
{
    protected float _casting = 0;
    [SerializeField] protected Spells spell;
    [SerializeField] protected int spellLevel;
    [SerializeField] public int casterInt = 1;
    [SerializeField] protected float castingSpeed;
    protected ISpells _spellInstance;

    private float lifeAtStartOfCast;
    private Character _currentEnemy;
    [HideInInspector] public bool shouldCast = false;

    protected override void Start()
    {
        base.Start();
        CastSetup();
    }

    protected void CastSetup()
    {
        Cast<Enemy> castState = new Cast<Enemy>(this, fsm);

        Transitions<Enemy> castTransition = new Transitions<Enemy>(castState);
        castTransition.AddCondition(() => stats.canCast);
        castTransition.AddCondition(() => playerFound);
        castTransition.AddCondition(() => !isCasting);
        castTransition.AddCondition(() => shouldCast);

        fsm.AddState(castState);
        fsm.AddTransition(castTransition);

        fsm.GetTransition(0).AddCondition(() => !shouldCast);
        StartCoroutine(RandomCasting());
        _spellInstance = new SpellInstanceWrapper(spell, spellLevel);
    }

    private IEnumerator RandomCasting() 
    {
        while (true)
        {
            if (Random.Range(0, 6) == 0)
                shouldCast = true;
            yield return new WaitForSeconds(6f);
        }
    }
    public override IEnumerator CastSpell(GameObject enemy)
    {
        isCasting = true;
        agent.isStopped = true;
        agent.ResetPath();
        animator.SetBool(animations[4], true);

        lifeAtStartOfCast = life;
        _currentEnemy = enemy.GetComponentInParent<Character>();

        while (_casting < 100 && lifeAtStartOfCast <= life)
        {
            this.ShowCastingCircle();
            if (_currentEnemy != null)
                _currentEnemy.BeingTargeted(true);

            _casting += 20 / (_spellInstance.Level * _spellInstance.CastDelayPerLevel) * Time.deltaTime * castingSpeed;

            Quaternion nextRotation = Quaternion.LookRotation(_currentEnemy.transform.position - transform.position);
            nextRotation.x = transform.rotation.x;
            if (nextRotation != Quaternion.identity)
                this.transform.localRotation = nextRotation;
            yield return null;
        }

        animator.SetBool(animations[4], false);
        _casting = 0;
        
        if (lifeAtStartOfCast <= life || enemy == null)
            animator.SetTrigger(animations[5]);
        

        this.HideCastingCircle();
        if (_currentEnemy != null)
            _currentEnemy.BeingTargeted(false);

        shouldCast = false;
        agent.isStopped = false;
        isCasting = false;
    }

    protected void ThrowSpell()
    {
        if (!(lifeAtStartOfCast > life))
            SpellController.Cast(_spellInstance, transform, _currentEnemy.transform, casterInt);

        this.HideCastingCircle();
        if (_currentEnemy != null)
            _currentEnemy.BeingTargeted(false);

        
    }
}
