using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : EnemyBase
{
    /// <summary>
    /// 플레이어의 컨트롤러
    /// </summary>
    PlayerInputController playerController;
    /// <summary>
    /// 애니메이션용 해시들
    /// </summary>
    readonly int Walk_Hash = Animator.StringToHash("Walk");
    readonly int Attack_Hash = Animator.StringToHash("Attack");
    readonly int Die_Hash = Animator.StringToHash("Die");
    /// <summary>
    /// 추적여부
    /// </summary>
    bool isChase = false;
    /// <summary>
    /// 공격여부
    /// </summary>
    bool isAttack = false;
    /// <summary>
    /// 플레이어를 볼 수 있는지
    /// </summary>
    bool isLook = false;
    /// <summary>
    /// 적 컴포넌트들
    /// </summary>
    NavMeshAgent agent;
    Animator animator;
    BoxCollider damageArea;
    /// <summary>
    /// 미사일 포트들
    /// </summary>
    Transform missilePort1;
    Transform missilePort2;
    /// <summary>
    /// 플레이어를 바라볼 방향
    /// </summary>
    Vector3 lookDirection;
    /// <summary>
    /// 점프 내려찍기 방향
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
    /// 플레이어의 이동방향을 예측하여 회전하는 함수
    /// </summary>
    /// <param name="input">플레이어의 움직임 input값</param>
    /// <param name="isMove">움직이고 있는지</param>
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
