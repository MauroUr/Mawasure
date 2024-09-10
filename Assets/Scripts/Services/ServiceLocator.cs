using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator instance;

    Dictionary<Type, MonoBehaviour> servicesByName = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
        
    }
    public T GetService<T>(Type serviceType) where T : MonoBehaviour
    {
        if (servicesByName.ContainsKey(serviceType))
            return servicesByName[serviceType] as T;
        else return null;    
    }
    public void SetService(Type serviceType, MonoBehaviour value)
    {
        servicesByName.Add(serviceType, value);
    }
}
