using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PurpleEnemy : EnemyBase
{
    /// <summary>
    /// �ִϸ��̼ǿ� �ؽõ�
    /// </summary>
    readonly int Walk_Hash = Animator.StringToHash("Walk");
    readonly int Attack_Hash = Animator.StringToHash("Attack");
    readonly int Die_Hash = Animator.StringToHash("Die");
    /// <summary>
    /// �����ϴ� ��(�ӵ�)
    /// </summary>
    public float dashPower=20.0f;
    /// <summary>
    /// ��������
    /// </summary>
    bool isChase = false;
    /// <summary>
    /// ���ݿ���
    /// </summary>
    bool isAttack = false;
    /// <summary>
    /// �� ������Ʈ��
    /// </summary>
    NavMeshAgent agent;
    Animator animator;
    BoxCollider damageArea;

    protected override void Awake()
    {
        base.Awake();
        agent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        damageArea = transform.GetChild(1).GetComponent<BoxCollider>();
    }

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(Chase), 3f);
    }
    private void Update()
    {
        if (agent.enabled)
        {
            agent.SetDestination(target.position);
            if (target != null  && agent.remainingDistance <= agent.stoppingDistance*2 && !isAttack)//�÷��̾� ���� ���ɰŸ��� ����������
            {
                animator.SetTrigger(Attack_Hash);
                StartCoroutine(Attack());
            }
            agent.isStopped = !isChase;
        }
    }


    private void FixedUpdate()
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
        }
        rb.angularVelocity = Vector3.zero;
    }

    protected override void Die()
    {
        animator.SetTrigger(Die_Hash);
        agent.enabled = false;
    }

    public void Chase()
    {
        isChase = true;
        animator.SetBool(Walk_Hash, true);
    }
    protected override IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        yield return new WaitForSeconds(0.1f);
        rb.AddForce(transform.forward * dashPower,ForceMode.VelocityChange);
        damageArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        rb.velocity=Vector3.zero;
        damageArea.enabled = false;
        yield return new WaitForSeconds(2.0f);
        isAttack = false;
        isChase = true;
    }
}