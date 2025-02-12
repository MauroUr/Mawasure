using System.Collections;
using UnityEngine;

public class Boss : Caster
{
    private bool _isAwake = false;

    [SerializeField] private GameObject winPanel;
    protected override void Start()
    {
        healthBar.maxValue = life;
        healthBar.value = life;
        agent.ResetPath();   
    }
    protected override void LateUpdate()
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
        _isAwake = true;
        FSMSetup();
        CastSetup();
        this.GetComponent<CapsuleCollider>().enabled = true;
    }

    public override void DestroySelf() 
    { 
        winPanel.SetActive(true);
        Destroy(gameObject); 
    }
}
