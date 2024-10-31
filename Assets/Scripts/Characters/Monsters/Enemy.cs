using System;
using System.Collections;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private float radius;
    private float _attackRadius;
    [SerializeField] private float damage;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackDelay;
    [SerializeField] private float experience;
    [SerializeField] private bool canCast;
    private float _lastAttack;
    private NavMeshAgent _agent;
    private Collider _playerFound;
    private States _state;
    private bool isIdleAfterRoam = false;
    
    private Timer angerTimer;
    private bool isAngry = false;
    
    private void Start()
    {
        this.life = 100;
        _agent = GetComponent<NavMeshAgent>();
        _attackRadius = _agent.stoppingDistance;
        angerTimer = new Timer(5000); 
        angerTimer.Elapsed += OnAngerTimerElapsed;
        angerTimer.AutoReset = false;
    }
    private void LateUpdate()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;

        CheckState();
    }

    private void ChangeState(States s)
    {
        _state = s;
    }

    private void CheckState()
    {
        switch (_state)
        {
            case States.IDLE:
                StartCoroutine(LookForPlayer());
                if (!_agent.pathPending)
                    animator.SetBool("Run", false);
                break;
            case States.ATTACK:
                if (_playerFound == null) break;

                _lastAttack += Time.deltaTime;

                float distance = Vector3.Distance(_playerFound.transform.position, this.transform.position);

                if(distance > radius && !isAngry)
                {
                    ChangeState(States.IDLE);
                    break;
                }

                if (distance < _attackRadius && _lastAttack > attackDelay)
                {
                    animator.SetTrigger("Attack");
                    _lastAttack = 0;
                    animator.SetBool("Run", false);
                    ChangeState(States.ATTACK);
                }
                else
                {
                    animator.SetBool("Run", true);
                    _agent.SetDestination(_playerFound.gameObject.transform.position);
                }
                
                break;
        }
    }

    public void MakeDamage()
    {
        _playerFound.gameObject.GetComponentInParent<Character>().TakeDamage(damage);
    }
    private IEnumerator LookForPlayer()
    {
        while (true)
        {
            _playerFound = Physics.OverlapSphere(transform.position, radius, 1 << 3).FirstOrDefault();

            if (_playerFound != null)
            {
                float distance = Vector3.Distance(_playerFound.transform.position, transform.position);
                if (distance > radius)
                {
                    _playerFound = null;
                }
                else if (_state != States.ATTACK)
                {
                    if (UnityEngine.Random.Range(0, 1) == 0 && canCast)
                        ChangeState(States.CAST);
                    else
                        ChangeState(States.ATTACK);
                    StopAllCoroutines();
                }
            }
            else if (!isIdleAfterRoam && UnityEngine.Random.Range(0, 5) == 0 && !_agent.pathPending)
                StartCoroutine(Roam());

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator Roam()
    {
        animator.SetBool("Run", true);

        float minRoamDistance = radius / 2; 
        float maxRoamDistance = radius;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minRoamDistance, maxRoamDistance);

        Vector3 roamPosition = new Vector3(randomDirection.x, 0, randomDirection.y) + transform.position;

        if (NavMesh.SamplePosition(roamPosition, out NavMeshHit hit, maxRoamDistance, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
            isIdleAfterRoam = true;

            yield return new WaitUntil(() => !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance);

            animator.SetBool("Run", false);
            yield return new WaitForSeconds(5f);

            isIdleAfterRoam = false;
        }

        yield break;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        _playerFound = Physics.OverlapSphere(transform.position, 100, 1 << 3).FirstOrDefault();
        StopAllCoroutines();
        StartAngerTimer(); 

        if (life <= 0)
        {
            Experience.Instance.AddXP(experience);
            Destroy(this.gameObject);
        }
    }
    private void StartAngerTimer()
    {
        ChangeState(States.ATTACK);
        isAngry = true;
        angerTimer.Start();
    }
    private void OnAngerTimerElapsed(object sender, ElapsedEventArgs e)
    {
        isAngry = false;
    }
    public enum States
    {
        IDLE,
        ATTACK,
        CAST
    }
}
