using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : Character
{
    [SerializeField] private float _radius;
    private float _attackRadius;
    [SerializeField] private float _damage;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float experience;
    private float _lastAttack;
    private NavMeshAgent _agent;
    private Collider _playerFound;
    private States _state;

    private void Start()
    {
        this.life = 100;
        _agent = GetComponent<NavMeshAgent>();
        _attackRadius = _agent.stoppingDistance;
        
        StartCoroutine(LookForPlayer());
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
                if(!_agent.pathPending)
                    _animator.SetBool("Run", false);
                break;
            case States.ATTACK:
                if (_playerFound == null) break;

                _lastAttack += Time.deltaTime;

                if (Vector3.Distance(_playerFound.transform.position, this.transform.position) < _attackRadius && _lastAttack > _attackDelay)
                {
                    _animator.SetTrigger("Attack");
                    _lastAttack = 0;
                    _animator.SetBool("Run", false);
                }
                else
                {
                    _animator.SetBool("Run", true);
                    _agent.SetDestination(_playerFound.gameObject.transform.position);
                }
                
                break;
        }
    }

    public void MakeDamage()
    {
        _playerFound.gameObject.GetComponentInParent<Character>().TakeDamage(_damage);
    }
    IEnumerator LookForPlayer()
    {
        while (true)
        {
            _playerFound = Physics.OverlapSphere(transform.position, _radius, 1 << 3).FirstOrDefault();

            if (_playerFound != null)
            {
                float distance = Vector3.Distance(_playerFound.transform.position, transform.position);
                if (distance > _radius)
                {
                    _playerFound = null;
                    ChangeState(States.IDLE);
                }
                else if (_state != States.ATTACK)
                    ChangeState(States.ATTACK);
            }
            else
                ChangeState(States.IDLE);
            

            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        _playerFound = Physics.OverlapSphere(transform.position, 100, 1 << 3).FirstOrDefault();
        _state = States.ATTACK;

        if (life <= 0)
        {
            Experience.Instance.AddXP(experience);
            Destroy(this.gameObject);
        }
    }

    public enum States
    {
        IDLE,
        ATTACK
    }
}
