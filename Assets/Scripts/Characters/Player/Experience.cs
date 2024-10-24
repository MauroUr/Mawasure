using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public static Experience Instance { get; private set; }
    private float _experience;
    private int _level;
    public event Action OnLevelUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _experience = 0;
        _level = Mathf.FloorToInt(0 / 1000);
    }

    public void AddXP(float experience)
    {
        _experience += experience;
        int prevLevel = _level;
        _level = Mathf.FloorToInt(_experience / 1000);

        if(prevLevel < _level)
            OnLevelUp.Invoke();
    }
}
