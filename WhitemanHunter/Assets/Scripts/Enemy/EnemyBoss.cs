using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    readonly int Shot_Hash = Animator.StringToHash("Shot");
    readonly int Taunt_Hash = Animator.StringToHash("Taunt");
    readonly int BigShot_Hash = Animator.StringToHash("BigShot");
    readonly int Die_Hash = Animator.StringToHash("Die");

    /// <summary>
    /// ȸ�� �ӵ�
    /// </summary>
    public float turnSmooth = 400.0f;

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
    /// �������� ����
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// �� ������Ʈ��
    /// </summary>
    NavMeshAgent agent;
    Animator animator;
    BoxCollider enemyMeshCollider;  //�� �޽ø� �����ϴ� �ݶ��̴�
    BoxCollider damageArea; //���� ���� �ݶ��̴�
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
        damageArea = transform.GetChild(3).GetComponent<BoxCollider>();
        enemyMeshCollider = GetComponent<BoxCollider>();
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
        tauntPosition = transform.position;
        StartCoroutine(RandomPattern());
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        
    }
    private void Update()
    {
        if (isDead)
            return;
        if (isLook)
        {
            rb.isKinematic = true;
            playerController.onMove += Prediction;
            transform.rotation = Quaternion.Slerp(  // ���� ��������� ȸ��
                transform.rotation,
                Quaternion.LookRotation(target.transform.position + lookDirection - transform.position),
                Time.deltaTime*turnSmooth);
        }
        else
        {
            agent.SetDestination(tauntPosition);
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

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    protected override void Die()
    {
        animator.SetTrigger(Die_Hash);
        agent.enabled = false;
        isDead = true;
        StopAllCoroutines();
    }

    /// <summary>
    /// ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Attack()
    {

        yield return null;
    }

    /// <summary>
    /// ������ �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
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
            isChase = false;
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
        }    
    }


    IEnumerator RandomPattern()
    {
        yield return new WaitForSeconds(0.1f);

        int randomAction = Random.Range(0, 3);

        switch (randomAction)
        {
            case 0:
                StartCoroutine(MissileShot());
                break;
            case 1:
                StartCoroutine(RockShot());
                break;
            case 2:
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        animator.SetTrigger(Shot_Hash);
        yield return new WaitForSeconds(0.2f);
        Factory.Instance.GetBossMissile(missilePort1.position, transform.forward);

        yield return new WaitForSeconds(0.3f);
        Factory.Instance.GetBossMissile(missilePort2.position, transform.forward);

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(RandomPattern());
    }

    IEnumerator RockShot()
    {
        rb.isKinematic = true;
        animator.SetTrigger(BigShot_Hash);
        Factory.Instance.GetBossRock(transform.position, transform.forward);
        isLook = false;
        yield return new WaitForSeconds(2.3f);
        isLook =true;
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(RandomPattern());
    }

    IEnumerator Taunt()
    {
        enemyMeshCollider.enabled = false;
        tauntPosition = target.position+lookDirection;
        isLook = false;
        agent.isStopped = false;
        rb.isKinematic = false;
        animator.SetTrigger(Taunt_Hash);

        yield return new WaitForSeconds(1.5f);
        damageArea.enabled = true;
        yield return new WaitForSeconds(0.5f);
        damageArea.enabled =false;
        yield return new WaitForSeconds(1f);

        isLook=true;
        rb.isKinematic = true;
        enemyMeshCollider.enabled = true;
        agent.isStopped = true;
        StartCoroutine(RandomPattern());
    }

   
}
