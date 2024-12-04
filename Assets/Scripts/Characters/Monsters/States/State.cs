
public abstract class State<T>
{
    protected T context;

    protected FiniteStateMachine<T> fsm;

    public State(T context, FiniteStateMachine<T> fsm)
    {
        this.context = context;
        this.fsm = fsm;
    }

    public abstract void Enter(State<T> prevState);
    public abstract void Tick();
    public abstract void Exit(State<T> nextState);
}

