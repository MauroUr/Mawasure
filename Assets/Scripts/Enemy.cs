using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Start()
    {
        this.life = 100;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        if(life <= 0)
            Destroy(this.gameObject);
    }
}
