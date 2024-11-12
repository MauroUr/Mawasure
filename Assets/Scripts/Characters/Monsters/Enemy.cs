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
    public State currentState { get; private set; }
    
    public bool isAngry { get; private set; }

    private new void Start()
    {
        base.Start();
        isAngry = false;
        agent = GetComponent<NavMeshAgent>();
        attackRadius = agent.stoppingDistance;
        currentState = new Idle(this);
        currentState.Enter(currentState);
    }
    protected void LateUpdate()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        currentState.Tick();
    }

    public void ChangeState(State s)
    {
        if (s is Cast && !canCast)
            return;

        currentState.Exit(s);
        s.Enter(currentState);
        currentState = s;
        
    }

    public virtual IEnumerator CastSpell(GameObject enemy)
    {
        throw new System.NotImplementedException("Cast method must be implemented in the subclass.");
    }

    public void MakeDamage()
    {
        playerFound.gameObject.GetComponentInParent<Character>().TakeDamage(damage);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (life <= 0)
        {
            Experience.Instance.AddXP(experience);
            Destroy(gameObject);
        }
        else
        {
            ChangeState(new Melee(this));
            StopCoroutine(nameof(HandleAngerMode));
            StartCoroutine(HandleAngerMode());
        }
    }
    private IEnumerator HandleAngerMode()
    {
        isAngry = true;
        yield return new WaitForSeconds(5f);
        isAngry = false;
    }
}