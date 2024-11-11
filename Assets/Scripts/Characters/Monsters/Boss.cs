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
        this.animator.SetBool("IsAwake", true);
        this._attackRadius += 1;
    }

    protected void SetAwake() { _isAwake = true; }
}
