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
    /// 현재 이동 방향
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// 플레이어의 트랜스폼
    /// </summary>
    Transform playerTransform;

    /// <summary>
    /// 방향 전환 회수 확인 및 설정용 프로퍼티
    /// </summary>
    int DirectionChangeCount
    {
        get => directionChangeCount;
        set
        {
            directionChangeCount = value;           // 값을 지정하고
        }
    }


    protected override void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * direction);    // direction 방향으로 이동
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 방향전환 회수가 남아있고, 부딪친 대상이 보더라면 처리
        if (DirectionChangeCount > 0 && collision.gameObject.CompareTag("BossArea"))
        {
            // 방향 전환
            direction = Vector2.Reflect(direction, collision.contacts[0].normal);   // 반사된 백터를 새로운 방향으로 설정
            DirectionChangeCount--;     // 방향 전환 회수 감소
        }
    }

    protected override void OnReset()
    {
        playerTransform = GameManager.Instance.Player.transform;    // 플레이어 찾아서 저장해 놓기
        int dirX= playerTransform.position.x - transform.position.x<0 ? 1:-1;
        

        direction = new Vector3(dirX,-1,0);                                 
        DirectionChangeCount = directionChangeMaxCount;             
    }

}
