using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    [SerializeField] private Boss boss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            boss.AwakeBoss();
            Destroy(this.gameObject);
        }

        
    }
}
