using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    private bool _isAwake = false;
    private float _casting = 0;
    [SerializeField] private Spells spell;
    private ISpells _spellInstance;
    [SerializeField] private GameObject winPanel;
    protected override void Start()
    {
        base.Start();
        currentState.Exit(currentState);
        currentState = null;
        agent.ResetPath();
        _spellInstance = new SpellInstanceWrapper(spell, 5);
    }
    protected new void LateUpdate()
    {
        if (_isAwake)
            base.LateUpdate();
    }
    public override IEnumerator CastSpell(GameObject enemy)
    {
        agent.ResetPath();
        animator.SetBool(animations[4], true);
        float startingLife = life;

        Character enemyCharacter = enemy.GetComponentInParent<Character>();

        while (_casting < 100 && startingLife <= life)
        {
            this.ShowCastingCircle();
            if (enemyCharacter != null)
                enemyCharacter.BeingTargeted(true);
            _casting += 20 / (spell.Level * spell.CastDelayPerLevel) * Time.deltaTime * 1.3f;

            if (_casting > 60 && animator.GetBool(animations[4]))
            {
                animator.SetBool(animations[4], false);
                animator.SetTrigger(animations[5]);
            }
            Quaternion nextRotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
            nextRotation.x = transform.rotation.x;
            if(nextRotation !=  Quaternion.identity) 
                this.transform.localRotation = nextRotation;
            yield return null;
        }
        _casting = 0;
        this.HideCastingCircle();
        if (enemyCharacter != null)
            enemyCharacter.BeingTargeted(false);

        animator.SetBool(animations[4], false);

        if (startingLife > life || enemy == null)
        {
            this.ChangeState(new Idle(this));
            yield break;
        }

        animator.ResetTrigger(animations[2]);
        SpellController.Cast(_spellInstance, transform, enemy.transform, 15);
        this.ChangeState(new Idle(this));
    }
    public void AwakeBoss()
    {
        this.animator.SetBool("IsAwake", true); 
    }

    protected void SetAwake() 
    {
        this.ChangeState(new Idle(this));
        _isAwake = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
    }

    public override void DestroySelf() 
    { 
        winPanel.SetActive(true);
        Destroy(gameObject); 
    }
}
