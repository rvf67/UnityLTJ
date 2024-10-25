using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GreenEnemy : EnemyBase
{
    readonly int Walk_Hash = Animator.StringToHash("Walk");
    readonly int Attack_Hash = Animator.StringToHash("Attack");
    readonly int Die_Hash = Animator.StringToHash("Die");
    bool isChase = false;
    NavMeshAgent agent;
    Animator animator;
    protected override void Awake()
    {
        base.Awake();
        agent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (isChase)
        {
            agent.SetDestination(target.position);
            if (target != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)//플레이어에게 도달했으면
            {
                animator.SetBool(Walk_Hash, false);
                animator.SetBool(Attack_Hash, true);
            }
            else
            {
                animator.SetBool(Walk_Hash, true);
                animator.SetBool(Attack_Hash, false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isChase = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            isChase = false;
            animator.SetBool(Walk_Hash,false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity=Vector3.zero;
    }

    protected override void Die()
    {
        animator.SetTrigger(Die_Hash);
    }
}
