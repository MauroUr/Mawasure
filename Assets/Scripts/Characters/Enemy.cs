using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private float radius;
    private NavMeshAgent agent;
    private Collider playerFound;

    private void Start()
    {
        this.life = 100;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        healthBar.gameObject.transform.rotation = Camera.main.transform.rotation;
        playerFound = Physics.OverlapSphere(transform.position, radius, LayerMask.NameToLayer("Player"))[0];

        if (playerFound != null && Vector3.Distance(playerFound.transform.position, this.transform.position) < radius)
        {
            agent.SetDestination(playerFound.transform.position);

            Vector3 direction = (playerFound.transform.position - this.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if(life <= 0)
            Destroy(this.gameObject);
    }
}
