using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : RecycleObject
{
    // HP는 1이고 터트렸을 때 점수는 0점

    // 생성되자마자 플레이어를 추적함(플레이어 방향으로 이동)
    // 자신의 트리거 안에 플레이어가 들어오면 그 후로 플레이어 추적 중지
    // 추적 정도를 설정할 수 있는 변수 만들기

    [Header("추적 미사일 데이터")]
    /// <summary>
    /// 미사일의 유도 성능. 높을 수록 빠르게 target방향으로 회전한다.
    /// </summary>
    public float guidedPerformance = 1.5f;

    /// <summary>
    /// 추적 대상
    /// </summary>
    Transform target;

    /// <summary>
    /// 추적 중인지 표시하는 변수(true면 추적중, false면 추적 중지)
    /// </summary>
    bool isGuided = true;

    protected override void OnReset()
    {
        base.OnReset();
        target = GameManager.Instance.Player.transform;
        isGuided = true;
    }
    public void OnMoveUpdate(float deltaTime)
    {
        if (isGuided)
        {
            Vector2 direction = target.position - transform.position;   // target 위치로 가는 방향
            
            // 플레이어쪽으로 천천히 회전하게 만들기
            transform.right = -Vector3.Slerp(-transform.right, direction, deltaTime * guidedPerformance);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGuided && collision.CompareTag("Player")) // 추적 중이고 플레이어가 범위안에 들어오면
        {
            isGuided = false;   // 이제 추적 중지
        }
    }


}
