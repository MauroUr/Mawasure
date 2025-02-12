using System.Collections;
using UnityEngine;

public class Melee<T> : State<T> where T : Enemy
{
    private float lastAttack;
    private Coroutine changeCoroutine;
    public Melee(T context, FiniteStateMachine<T> fsm) : base(context, fsm) 
    {
        lastAttack = context.stats.attackDelay +1f;
    }

    public override void Enter(State<T> prevState)
    {
        context.animator.ResetTrigger(context.animations[2]);
        if (prevState is Idle<Enemy>)
            context.stats.radius *= 2;
        if(context.stats.canCast)
            changeCoroutine = context.StartCoroutine(ShouldChangeAttack());

        context.animator.SetBool(context.animations[0], true);
    }

    public override void Exit(State<T> nextState)
    {
        if (changeCoroutine != null)
            context.StopCoroutine(changeCoroutine);
        context.animator.SetBool(context.animations[0], false);
        return;
    }
    private IEnumerator ShouldChangeAttack()
    {
        while (true)
        {
            if (lastAttack > context.stats.attackDelay + 3f)
                   lastAttack = context.stats.attackDelay + 0.01f;
            
            yield return null;
        }
    }

    public override void Tick()
    {
        if (context.playerFound == null) return;

        lastAttack += Time.deltaTime;

        float distance = Vector3.Distance(context.playerFound.transform.position, context.transform.position);

        if (distance < context.stats.attackRadius && lastAttack > context.stats.attackDelay)
        {
            context.animator.SetTrigger(context.animations[1]);
            lastAttack = 0;
            
        }
        else if (distance > context.stats.attackRadius)
            context.agent.SetDestination(context.playerFound.gameObject.transform.position);
    }

}
