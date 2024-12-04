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
    protected FiniteStateMachine<Enemy> fsm;
    public bool isAngry { get; private set; }
    protected bool isCasting = false;

    protected override void Start()
    {
        base.Start();
        isAngry = false;
        agent = GetComponent<NavMeshAgent>();
        attackRadius = agent.stoppingDistance;

        Idle<Enemy> idleState = new Idle<Enemy>(this, fsm);
        Melee<Enemy> meleeState = new Melee<Enemy>(this, fsm);


        List<State<Enemy>> states = new List<State<Enemy>>();
        states.Add(idleState);
        states.Add(meleeState);

        fsm = new FiniteStateMachine<Enemy>(states);

        Transitions<Enemy> meleeTransition = new Transitions<Enemy>(meleeState);
        meleeTransition.AddCondition(() => playerFound);
        meleeTransition.AddCondition(() => !isCasting);
        fsm.AddTransition(meleeTransition);

        Transitions<Enemy> idleTransition = new Transitions<Enemy>(idleState);
        idleTransition.AddCondition(() =>
        {
            if (playerFound == null)
                return false;

            float distance = Vector3.Distance(playerFound.transform.position, transform.position);
            return distance > radius && !isAngry;
        });
        fsm.AddTransition(idleTransition);

    }

    protected virtual void LateUpdate()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        fsm.Tick();
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
                    TryFindPlayer();
            }
        }
        
    }

    public bool TryFindPlayer()
    {
        Collider potentialPlayer = Physics.OverlapSphere(transform.position, isAngry ? 100 : radius, 1 << LayerMask.NameToLayer("Player")).FirstOrDefault();

        if (potentialPlayer == null && playerFound != null)
            playerFound = null;
        else if (potentialPlayer != null)
            playerFound = potentialPlayer;

        return playerFound != null;
    }

    public virtual void DestroySelf() { Destroy(gameObject); }
    private IEnumerator HandleAngerMode()
    {
        isAngry = true;
        yield return new WaitForSeconds(5f);
        isAngry = false;
    }

    public virtual IEnumerator CastSpell(GameObject enemy)
    {
        throw new System.NotImplementedException("Cast method must be implemented in the subclass.");
    }

    public virtual void CancelCasting()
    {
        throw new System.NotImplementedException("Cancel cast method must be implemented in the subclass.");
    }

}