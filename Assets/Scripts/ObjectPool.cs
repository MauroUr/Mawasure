
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Behaviour
{
    private LinkedList<T> _TStates;
    public int Count = 0;

    public ObjectPool(){
        _TStates = new LinkedList<T>();
    }

    public void Register(T T)
    {
        _TStates.AddLast(T);
        Count++;
    }

    public T SearchFor(Func<T, bool> comparer)
    {
        foreach (T t in _TStates)
            if (comparer(t))
                return t;

        return null;
    }

    public T FindInactive()
    {
        if (_TStates.Count == 0)
            return null;
        T lastValue = _TStates.Last.Value;
        _TStates.RemoveLast();
        Count--;
        return lastValue;
    }
}
