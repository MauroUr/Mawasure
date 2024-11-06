using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float life;
    [SerializeField] protected Slider healthBar;

    protected void Start()
    {
        healthBar.maxValue = life;
        healthBar.value = life;
    }
    public virtual void TakeDamage(float damage)
    {
        this.life -= damage;
        healthBar.value = life;
    }

}
