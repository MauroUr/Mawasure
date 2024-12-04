using System;
using System.Collections.Generic;

public class Transitions<T>
{
    private readonly List<Func<bool>> conditions = new();
    public State<T> ToState { get; }

    public Transitions(State<T> toState)
    {
        ToState = toState;
    }

    public void AddCondition(Func<bool> condition)
    {
        conditions.Add(condition);
    }

    public bool CheckConditions(State<T> currentState)
    {
        if (currentState.GetType() == ToState.GetType())
            return false;
        foreach (var condition in conditions)
        {
            if (!condition.Invoke())
                return false;
        }
        return true;
    }
}
