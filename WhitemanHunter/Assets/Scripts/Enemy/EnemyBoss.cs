using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : EnemyBase
{
    /// <summary>
    /// �÷��̾��� ��Ʈ�ѷ�
    /// </summary>
    PlayerInputController playerController;
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
    /// �÷��̾ �� �� �ִ���
    /// </summary>
    bool isLook = false;
    /// <summary>
    /// �� ������Ʈ��
    /// </summary>
    NavMeshAgent agent;
    Animator animator;
    BoxCollider damageArea;
    /// <summary>
    /// �̻��� ��Ʈ��
    /// </summary>
    Transform missilePort1;
    Transform missilePort2;
    /// <summary>
    /// �÷��̾ �ٶ� ����
    /// </summary>
    Vector3 lookDirection;
    /// <summary>
    /// ���� ������� ����
    /// </summary>
    Vector3 tauntPosition;
    protected override void Awake()
    {
        base.Awake();
        agent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        missilePort1 = transform.GetChild(1);
        missilePort2 = transform.GetChild(2);
    }
    protected override void Start()
    {
        base.Start();
        playerController = target.GetComponent<PlayerInputController>();
        isLook = true;
    }
    protected override void OnReset()
    {
        isLook = true;
    }
    private void Update()
    {
        if (isLook)
        {
            playerController.onMove += Prediction;
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


    protected override void Die()
    {
        animator.SetTrigger(Die_Hash);
        agent.enabled = false;
    }

    protected override IEnumerator Attack()
    {

        yield return null;
    }

 
    protected override IEnumerator OnDamage()
    {
        MeshsChange(bodyMeshs, Color.red);
        yield return new WaitForSeconds(0.1f);
        if (health > 0)
        {
            MeshsChange(bodyMeshs, Color.white);
        }
        else
        {
            MeshsChange(bodyMeshs, Color.gray);
            gameObject.layer = enemyUndieLayer;
            Die();
        }
    }

    /// <summary>
    /// �÷��̾��� �̵������� �����Ͽ� ȸ���ϴ� �Լ�
    /// </summary>
    /// <param name="input">�÷��̾��� ������ input��</param>
    /// <param name="isMove">�����̰� �ִ���</param>
    public void Prediction(Vector2 input, bool isMove)
    {
        if (isMove)
        {
            float h = input.x;
            float v = input.y;
            lookDirection = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookDirection);
        }    
    }
}
