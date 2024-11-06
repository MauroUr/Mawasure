using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> fence;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private Boss boss;
   
    public void RemoveFence()
    {
        foreach (MeshRenderer f in fence)
            f.enabled = false;

        foreach (Collider collider in colliders)
            collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
            boss.AwakeBoss();
    }
}
