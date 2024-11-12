using UnityEngine;

public class Melee : State
{
    private float lastAttack;
    public Melee(Enemy enemy) : base(enemy) 
    {
        lastAttack = enemy.attackDelay+1;
    }

    public override void Enter(State prevState)
    {
        if(prevState is Idle)
            enemy.radius *= 2;
    }

    public override void Exit(State nextState)
    {
        return;
    }

    public override void Tick()
    {
        if (enemy.playerFound == null) return;

        lastAttack += Time.deltaTime;

        float distance = Vector3.Distance(enemy.playerFound.transform.position, enemy.transform.position);

        if (distance > enemy.radius && !enemy.isAngry)
        {
            enemy.ChangeState(new Idle(enemy));
            return;
        }

        if (distance < enemy.attackRadius && lastAttack > enemy.attackDelay)
        {
            enemy.animator.SetTrigger(enemy.animations[1]);
            lastAttack = 0;
            enemy.animator.SetBool(enemy.animations[0], false);
        }
        else if (distance > enemy.attackRadius)
        {
            enemy.animator.SetBool(enemy.animations[0], true);
            enemy.agent.SetDestination(enemy.playerFound.gameObject.transform.position);
        }
    }
}
