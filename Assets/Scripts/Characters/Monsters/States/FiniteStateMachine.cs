using System;
using System.Collections.Generic;

public class FiniteStateMachine<T>
{
    public State<T> CurrentState { get; private set; }
    private readonly List<Transitions<T>> transitions = new();
    private Dictionary<Type, State<T>> _states = new Dictionary<Type, State<T>>();

    /// <summary>
    /// First State in the list will be the initial State.
    /// </summary>
    public FiniteStateMachine( List<State<T>> states)
    {
        foreach (State<T> state in states)
        {
            Type stateType = state.GetType();
            if (!_states.ContainsKey(stateType))
                _states.Add(stateType, state);
        }
        CurrentState = states[0];
        CurrentState.Enter(null); 
    }

    public void AddTransition(Transitions<T> transition)
    {
        transitions.Add(transition);
    }

    public void AddState(State<T> state)
    {
        Type stateType = state.GetType();
        if (!_states.ContainsKey(stateType))
            _states.Add(stateType, state);

    }

    public void ChangeState(Type stateType)
    {
        if (!_states.TryGetValue(stateType, out State<T> newState))
            return;

        CurrentState?.Exit(newState);
        State<T> prevState = CurrentState;
        CurrentState = newState;
        CurrentState.Enter(prevState);
    }


    public void Tick()
    {
        foreach (var transition in transitions)
        {
            if (transition.CheckConditions(CurrentState))
            {
                ChangeState(transition.ToState.GetType());
                return;
            }
        }

        CurrentState.Tick();
    }


}
