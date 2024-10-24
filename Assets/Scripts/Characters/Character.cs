using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    protected float life;
    protected float speed;
    [SerializeField] protected Slider healthBar;

    public virtual void TakeDamage(float damage)
    {
        this.life -= damage;
        healthBar.value = life;
    }

}
