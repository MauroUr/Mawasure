using UnityEngine;

public class Cast : State
{
    private Coroutine castCoroutine;
    public Cast(Enemy enemy) : base(enemy) { }
    public override void Enter(State prevState)
    {
        if (prevState is Idle)
            enemy.radius *= 2;
        castCoroutine = enemy.StartCoroutine(enemy.CastSpell(enemy.playerFound.gameObject));
    }

    public override void Exit(State nextState)
    {
        if (castCoroutine != null)
            enemy.StopCoroutine(castCoroutine);
        return;
    }

    public override void Tick()
    {
        return;
    }
}
