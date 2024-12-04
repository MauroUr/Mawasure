
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
    [SerializeField] public bool canCast { get; }
    [SerializeField] public float attackRadius { get; }

    [SerializeField] public float attackDelay { get; }
    [SerializeField] public Transform transform { get; }
    [SerializeField] public List<string> animations { get; }
    [SerializeField] public Animator animator { get; }

    [SerializeField] public float radius {  get; set; }
    [SerializeField] public Collider playerFound { get; set; }
    [SerializeField] public NavMeshAgent agent { get; }
    [SerializeField] public IEnumerator CastSpell(GameObject enemy);

    public void MakeDamage();

    public Coroutine StartCoroutine(IEnumerator methodName);
    public void StopCoroutine(Coroutine routine);
    public void TakeDamage(float damage);

    public bool TryFindPlayer();
    public void DestroySelf();
}
