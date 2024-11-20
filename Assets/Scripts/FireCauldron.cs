using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCauldron : MonoBehaviour
{
    [SerializeField] private GameObject thisFire;
    [SerializeField] private GameObject otherFire;
    [SerializeField] private GameObject fence;
    [SerializeField] private Outline thisOutliner;
    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
        {
            thisFire.SetActive(true);
            this.thisOutliner.enabled = false;
            if (thisFire.activeSelf && otherFire.activeSelf)
                fence.SetActive(false);
        }
    }
}
