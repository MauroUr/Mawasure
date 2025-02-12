using System.Collections;
using UnityEngine;

public class CoroutineRunner : Service
{
    public void StartRoutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
