using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class Idle : State
{
    private bool isIdleAfterRoam = false;
    private Coroutine lookForPlayerCoroutine;
    private Coroutine roamCoroutine;

    public Idle(Enemy enemy) : base(enemy) { }

    public override void Enter(State prevState)
    {
        if (prevState is Melee || prevState is Cast)
            enemy.radius /= 2;

        lookForPlayerCoroutine = enemy.StartCoroutine(LookForPlayer());
    }

    public override void Exit(State nextState)
    {
        if (lookForPlayerCoroutine != null)
            enemy.StopCoroutine(lookForPlayerCoroutine);

        if (roamCoroutine != null)
            enemy.StopCoroutine(roamCoroutine);
    }

    public override void Tick()
    {
        
    }

    private IEnumerator LookForPlayer()
    {
        while (true)
        {
            enemy.playerFound = Physics.OverlapSphere(enemy.transform.position, enemy.radius, 1 << 3).FirstOrDefault();

            if (enemy.playerFound != null)
            {
                if (enemy.currentState is not Melee && enemy.currentState is not Cast)
                {
                    if (Random.Range(0, 6) == 0 && enemy.canCast)
                        enemy.ChangeState(new Cast(enemy));
                    else
                        enemy.ChangeState(new Melee(enemy));

                    yield break;
                }
            }
            else if (!isIdleAfterRoam && Random.Range(0, 5) == 0 && !enemy.agent.pathPending)
            {
                if (roamCoroutine == null)
                    roamCoroutine = enemy.StartCoroutine(Roam());
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator Roam()
    {
        enemy.animator.SetBool(enemy.animations[0], true);

        float minRoamDistance = enemy.radius / 2;
        float maxRoamDistance = enemy.radius;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minRoamDistance, maxRoamDistance);

        Vector3 roamPosition = new Vector3(randomDirection.x, 0, randomDirection.y) + enemy.transform.position;

        if (NavMesh.SamplePosition(roamPosition, out NavMeshHit hit, maxRoamDistance, NavMesh.AllAreas))
        {
            enemy.agent.SetDestination(hit.position);
            isIdleAfterRoam = true;

            yield return new WaitUntil(() => !enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance);

            enemy.animator.SetBool(enemy.animations[0], false);
            yield return new WaitForSeconds(5f);

            isIdleAfterRoam = false;
        }

        roamCoroutine = null;
        yield break;
    }
}
