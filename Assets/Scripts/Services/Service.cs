using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Service : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(SuscribeService());
    }

    private IEnumerator SuscribeService()
    {
        while (ServiceLocator.instance == null)
            yield return null;

        ServiceLocator.instance.SetService(GetType(), this);
    }
}
