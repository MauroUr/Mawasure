using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Idle<T> : State<T> where T : Enemy
{
    private bool isIdleAfterRoam = false;
    private Coroutine lookForPlayerCoroutine;
    private Coroutine roamCoroutine;

    public Idle(T context, FiniteStateMachine<T> fsm) : base(context, fsm) { }

    public override void Enter(State<T> prevState)
    {
        if (prevState is Melee<T> || prevState is Cast<T>)
            context.radius /= 2;

        context.agent.ResetPath();
        lookForPlayerCoroutine = context.StartCoroutine(LookForPlayer());
    }

    public override void Exit(State<T> nextState)
    {
        if (lookForPlayerCoroutine != null)
            context.StopCoroutine(lookForPlayerCoroutine);

        if (roamCoroutine != null)
            context.StopCoroutine(roamCoroutine);
    }

    public override void Tick()
    {
        
    }

    private IEnumerator LookForPlayer()
    {
        while (true)
        {
            context.TryFindPlayer();

            if (!isIdleAfterRoam && Random.Range(0, 5) == 0 && !context.agent.pathPending)
            {
                if (roamCoroutine == null)
                    roamCoroutine = context.StartCoroutine(Roam());
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator Roam()
    {
        context.animator.SetBool(context.animations[0], true);

        float minRoamDistance = context.radius / 2;
        float maxRoamDistance = context.radius;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minRoamDistance, maxRoamDistance);

        Vector3 roamPosition = new Vector3(randomDirection.x, 0, randomDirection.y) + context.transform.position;

        if (NavMesh.SamplePosition(roamPosition, out NavMeshHit hit, maxRoamDistance, NavMesh.AllAreas))
        {
            context.agent.SetDestination(hit.position);
            isIdleAfterRoam = true;

            yield return new WaitUntil(() => !context.agent.pathPending && context.agent.remainingDistance <= context.agent.stoppingDistance);

            context.animator.SetBool(context.animations[0], false);
            yield return new WaitForSeconds(5f);

            isIdleAfterRoam = false;
        }

        roamCoroutine = null;
        yield break;
    }
}
