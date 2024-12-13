using Unity.VisualScripting;
using UnityEngine;

public class Cast<T> : State<T> where T : Enemy
{
    private Coroutine castCoroutine;
    public Cast(T context, FiniteStateMachine<T> fsm) : base(context, fsm) { }
    public override void Enter(State<T> prevState)
    {
        if (prevState is Idle<T>)
            context.stats.radius *= 2;

        castCoroutine = context.StartCoroutine(context.CastSpell(context.playerFound.gameObject));
    }

    public override void Exit(State<T> nextState)
    {
        if (castCoroutine != null)
            context.StopCoroutine(castCoroutine);

        return;
    }

    public override void Tick()
    {
        float distance = Vector3.Distance(context.playerFound.transform.position, context.transform.position);
        if (distance > context.stats.radius && !context.isAngry)
        {
            fsm.ChangeState(typeof(Idle<T>));
            context.CancelCasting();
            return;
        }
    }
}
