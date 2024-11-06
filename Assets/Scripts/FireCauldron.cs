using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCauldron : MonoBehaviour
{
    [SerializeField] private GameObject thisFire;
    [SerializeField] private GameObject otherFire;
    [SerializeField] private Fence fence;
    private void OnTriggerEnter(Collider other)
    {
        thisFire.SetActive(true);

        if (thisFire.activeSelf && otherFire.activeSelf)
            fence.RemoveFence();
    }
}
