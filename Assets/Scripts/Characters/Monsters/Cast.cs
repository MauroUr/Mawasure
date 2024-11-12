
public class Cast : State
{
    public Cast(Enemy enemy) : base(enemy) { }
    public override void Enter(State prevState)
    {
        if (prevState is Idle)
            enemy.radius *= 2;
        enemy.CastSpell(enemy.playerFound.gameObject);
    }

    public override void Exit(State nextState)
    {
        return;
    }

    public override void Tick()
    {
        return;
    }
}
