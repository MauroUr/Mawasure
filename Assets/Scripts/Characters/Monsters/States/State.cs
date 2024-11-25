
public abstract class State
{
    protected Enemy enemy;
    
    public State(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public abstract void Enter(State prevState);
    public abstract void Tick();
    public abstract void Exit(State nextState);
}
