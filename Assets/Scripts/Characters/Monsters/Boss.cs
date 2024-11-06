using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private bool _isAwake = false;

    protected new void LateUpdate()
    {
        if (_isAwake)
        {
            base.LateUpdate();
        
        }
    }

    public void AwakeBoss()
    {
        _isAwake = true;
        this.animator.SetBool("IsAwake", true);
    }
}
