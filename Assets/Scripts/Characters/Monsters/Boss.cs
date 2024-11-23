using System.Collections;
using UnityEngine;

public class Boss : Caster
{
    private bool _isAwake = false;

    [SerializeField] private GameObject winPanel;
    protected override void Start()
    {
        base.Start();
        currentState.Exit(currentState);
        currentState = null;
        agent.ResetPath();
        
    }
    protected new void LateUpdate()
    {
        if (_isAwake)
            base.LateUpdate();
    }

    public void AwakeBoss()
    {
        this.animator.SetBool("IsAwake", true); 
    }

    protected void SetAwake() 
    {
        this.ChangeState(new Idle(this));
        _isAwake = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
    }

    public override void DestroySelf() 
    { 
        winPanel.SetActive(true);
        Destroy(gameObject); 
    }
}
