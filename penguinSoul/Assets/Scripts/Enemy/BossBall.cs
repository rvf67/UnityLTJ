using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : EnemyBase
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 방향 전환이 가능한 최대 회수
    /// </summary>
    public int directionChangeMaxCount = 5;

    /// <summary>
    /// 현재 방향 남은 회수
    /// </summary>
    int directionChangeCount = 0;

    /// <summary>
    /// 라인렌더러
    /// </summary>
    LineRenderer lineRenderer;
    /// <summary>
    /// 현재 이동 방향
    /// </summary>
    Vector2 direction;

    /// <summary>
    /// 플레이어의 트랜스폼
    /// </summary>
    Transform playerTransform;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D rb;


    /// <summary>
    /// 방향 전환 회수 확인 및 설정용 프로퍼티
    /// </summary>
    int DirectionChangeCount
    {
        get => directionChangeCount;
        set
        {
            directionChangeCount = value;           // 값을 지정하고
            if (directionChangeCount == 0)
            {
                int copy = point;
                point = 0;
                Die();
                point = copy;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rb= GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position+Time.fixedDeltaTime * moveSpeed * direction);    // direction 방향으로 이동
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 방향전환 회수가 남아있고, 부딪친 대상이 보스지역이라면 처리
        if (DirectionChangeCount > 0 && collision.gameObject.CompareTag("BossArea"))
        {
            // 방향 전환
            direction = Vector2.Reflect(direction, collision.contacts[0].normal);   // 반사된 백터를 새로운 방향으로 설정
            DirectionChangeCount--;     // 방향 전환 회수 감소
        }
    }

    protected override void OnReset()
    {        
        base.OnReset();
        DirectionChangeCount = directionChangeMaxCount;    
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

}
