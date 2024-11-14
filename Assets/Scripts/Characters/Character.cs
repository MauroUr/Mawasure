using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float life;
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected GameObject castCircleTarget;
    [SerializeField] protected GameObject castCircle;

    protected virtual void Start()
    {
        healthBar.maxValue = life;
        healthBar.value = life;
    }
    public virtual void TakeDamage(float damage)
    {
        this.life -= damage;
        healthBar.value = life;
    }

    public void BeingTargeted(bool isTargeted)
    {
        castCircleTarget.SetActive(isTargeted);
    }

    protected void ShowCastingCircle()
    {
        castCircle.SetActive(true);
    }
    protected void HideCastingCircle()
    {
        castCircle.SetActive(false);
    }
}
