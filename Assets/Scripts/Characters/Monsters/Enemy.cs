using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] public List<string> animations = new();
    [SerializeField] public float radius;
    public float attackRadius;

    [SerializeField] private float damage;
    [SerializeField] public Animator animator;
    [SerializeField] public float attackDelay;
    
    [SerializeField] private float experience;
    [SerializeField] public bool canCast;

    public NavMeshAgent agent;
    public Collider playerFound { get; set; }
    public State currentState { get; protected set; }
    public bool isAngry { get; private set; }

    protected override void Start()
    {
        base.Start();
        isAngry = false;
        agent = GetComponent<NavMeshAgent>();
        attackRadius = agent.stoppingDistance;
        EvaluateStateChange();
    }

    protected virtual void LateUpdate()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        currentState.Tick();
    }

    public void ChangeState(State s)
    {
        if (s is Cast && !canCast)
            return;

        if(currentState != null)
            currentState.Exit(s);
        
        State prevState = currentState;
        currentState = s;
        currentState.Enter(prevState);

    }

    public virtual IEnumerator CastSpell(GameObject enemy)
    {
        throw new System.NotImplementedException("Cast method must be implemented in the subclass.");
    }

    public void MakeDamage()
    {
        playerFound.gameObject.GetComponentInParent<Character>().TakeDamage(damage);
        animator.ResetTrigger(animations[2]);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (!animator.GetBool(animations[3])) 
        {
            animator.ResetTrigger(animations[2]);
            animator.SetTrigger(animations[2]);

            if (life <= 0)
            {
                Experience.Instance.AddXP(experience);
                animator.ResetTrigger(animations[2]);
                animator.SetBool(animations[3], true);
                healthBar.gameObject.SetActive(false);
                agent.ResetPath();
                this.enabled = false;
            }
            else
            {
                StopCoroutine(nameof(HandleAngerMode));
                StartCoroutine(HandleAngerMode());
                if (playerFound == null)
                {
                    TryFindPlayer();
                    EvaluateStateChange();
                }
            }
        }
        
    }
    public void EvaluateStateChange()
    {
        if (playerFound == null)
            ChangeState(new Idle(this));
        else if (canCast && Random.Range(0, 6) == 0)
            ChangeState(new Cast(this));
        else
            ChangeState(new Melee(this));
    }

    public bool TryFindPlayer()
    {
        if(isAngry)
            playerFound = Physics.OverlapSphere(transform.position, 100, 1 << LayerMask.NameToLayer("Player")).FirstOrDefault();
        else
            playerFound = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Player")).FirstOrDefault();

        return playerFound != null;
    }


    public virtual void DestroySelf() { Destroy(gameObject); }
    private IEnumerator HandleAngerMode()
    {
        isAngry = true;
        yield return new WaitForSeconds(5f);
        isAngry = false;
    }
}