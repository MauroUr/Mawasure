using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private void Start()
    {
        this.life = 100;
    }
    private void Update()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        if(life <= 0)
            Destroy(this.gameObject);
    }
}
