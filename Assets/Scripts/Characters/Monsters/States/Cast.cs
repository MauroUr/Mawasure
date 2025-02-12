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

    }
}
