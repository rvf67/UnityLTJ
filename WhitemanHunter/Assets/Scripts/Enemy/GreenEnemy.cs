using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �ɷ��� ���� ��
/// </summary>
public class GreenEnemy : EnemyBase
{
    /// <summary>
    /// �ִϸ��̼ǿ� �ؽõ�
    /// </summary>
    readonly int Walk_Hash = Animator.StringToHash("Walk");
    readonly int Attack_Hash = Animator.StringToHash("Attack");
    readonly int Die_Hash = Animator.StringToHash("Die");
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
            agent.isStopped = !isChase;
            if (target != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !isAttack)//�÷��̾�� ����������
            {
                animator.SetTrigger(Attack_Hash);
                StartCoroutine(Attack());
            }
        }
    }

    private void FixedUpdate()
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    public void Chase()
    {
        isChase = true;
        animator.SetBool(Walk_Hash, true);
    }
    protected override void Die()
    {
        base.Die();
        animator.SetTrigger(Die_Hash);
        agent.enabled = false;
        GameManager.Instance.GamePanel.EnemyCntA--;
    }

    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);
        isAttack = true;
        damageArea.enabled = true;
        yield return new WaitForSeconds(1.0f);
        damageArea.enabled = false;
        yield return new WaitForSeconds(1.0f);
        isAttack = false;
    }
}
