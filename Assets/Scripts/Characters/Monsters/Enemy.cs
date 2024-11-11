using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] protected List<string> animations = new();
    [SerializeField] private float radius;
    protected float _attackRadius;
    [SerializeField] private float damage;
    [SerializeField] protected Animator animator;
    [SerializeField] private float attackDelay;
    [SerializeField] private float experience;
    [SerializeField] private bool canCast;
    private float _lastAttack;
    private NavMeshAgent _agent;
    private Collider _playerFound;
    private States _state;
    private bool _isIdleAfterRoam = false;
    
    private bool _isAngry = false;
    
    private new void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _attackRadius = _agent.stoppingDistance;
        StartCoroutine(LookForPlayer());
    }
    protected void LateUpdate()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        CheckState();
    }

    private void ChangeState(States s)
    {
        if (_state == States.IDLE && s == States.ATTACK)
            radius *= 2;
        else if (_state == States.ATTACK && s == States.IDLE)
            radius /= 2;

        if (s == States.IDLE && _state != States.IDLE)
        {
            _isIdleAfterRoam = false;
            StartCoroutine(LookForPlayer());
        }

        _state = s;
    }

    private void CheckState()
    {
        switch (_state)
        {
            case States.IDLE:
                
                break;
            case States.ATTACK:
                HandleAttackState();
                break;
            
        }
    }

    protected virtual IEnumerator CastSpell(GameObject enemy)
    {
        throw new System.NotImplementedException("Cast method must be implemented in the subclass.");
    }

    private void HandleAttackState()
    {
        if (_playerFound == null) return;

        _lastAttack += Time.deltaTime;

        float distance = Vector3.Distance(_playerFound.transform.position, this.transform.position);

        if (distance > radius && !_isAngry)
        {
            ChangeState(States.IDLE);
            return;
        }

        if (distance < _attackRadius && _lastAttack > attackDelay)
        {
            animator.SetTrigger(animations[1]);
            _lastAttack = 0;
            animator.SetBool(animations[0], false);
        }
        else if (distance > _attackRadius)
        {
            animator.SetBool(animations[0], true);
            _agent.SetDestination(_playerFound.gameObject.transform.position);
        }
    }
    public void MakeDamage()
    {
        _playerFound.gameObject.GetComponentInParent<Character>().TakeDamage(damage);
    }
    protected IEnumerator LookForPlayer()
    {
        while (true)
        {
            _playerFound = Physics.OverlapSphere(transform.position, radius, 1 << 3).FirstOrDefault();

            if (_playerFound != null)
            {
                float distance = Vector3.Distance(_playerFound.transform.position, transform.position);
                
                if (distance > radius)
                    _playerFound = null;
                else if (_state != States.ATTACK)
                {
                    if (Random.Range(0, 1) == 0 && canCast)
                        ChangeState(States.CAST);
                    else
                        ChangeState(States.ATTACK);
                    StopAllCoroutines();
                }
            }
            else if (!_isIdleAfterRoam && Random.Range(0, 5) == 0 && !_agent.pathPending)
                StartCoroutine(Roam());

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator Roam()
    {
        animator.SetBool(animations[0], true);

        float minRoamDistance = radius / 2; 
        float maxRoamDistance = radius;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minRoamDistance, maxRoamDistance);

        Vector3 roamPosition = new Vector3(randomDirection.x, 0, randomDirection.y) + transform.position;

        if (NavMesh.SamplePosition(roamPosition, out NavMeshHit hit, maxRoamDistance, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
            _isIdleAfterRoam = true;

            yield return new WaitUntil(() => !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance);

            animator.SetBool(animations[0], false);
            yield return new WaitForSeconds(5f);

            _isIdleAfterRoam = false;
        }

        yield break;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (life <= 0)
        {
            Experience.Instance.AddXP(experience);
            Destroy(gameObject);
        }
        else
        {
            ChangeState(States.ATTACK);
            StopCoroutine(nameof(HandleAngerMode));
            StartCoroutine(HandleAngerMode());
        }
    }
    private IEnumerator HandleAngerMode()
    {
        _isAngry = true;
        yield return new WaitForSeconds(5f);
        _isAngry = false;
    }

    public enum States
    {
        IDLE,
        ATTACK,
        CAST
    }
}
