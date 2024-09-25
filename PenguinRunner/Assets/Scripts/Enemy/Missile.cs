using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : EnemyBase
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

    GameOverTime gameOverTime;
    /// <summary>
    /// 추적 중인지 표시하는 변수(true면 추적중, false면 추적 중지)
    /// </summary>
    bool isGuided = true;

    /// <summary>
    /// 죽을때 발생하는 이팩트
    /// </summary>
    GameObject flash;
    SpriteRenderer spriteRenderer;
    int flipXint;
    private bool isDie;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        gameOverTime = GameManager.Instance.GameOverTime;
        flash= transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);
    }
    protected override void OnReset()
    {
        base.OnReset();
        isDie = false;
        target = GameManager.Instance.Player.transform;
        gameOverTime = GameManager.Instance.GameOverTime;
        isGuided = true;
    }
    protected override void OnMoveUpdate(float deltaTime)
    {
        if (isGuided)
        {
            Vector2 direction = (target.position - transform.position).normalized;   // target 위치로 가는 방향
            
            // 플레이어쪽으로 천천히 회전하게 만들기
            transform.right = flipXint*Vector3.Slerp(flipXint*transform.right, direction, deltaTime * guidedPerformance);
            transform.Translate(deltaTime * moveSpeed * gameOverTime.level * direction, Space.World);
        }
    }

    public void breakMissile()
    {
        StartCoroutine(DestroyMissile());
    }
    IEnumerator DestroyMissile()
    {
        if (!isDie)
        {
            isDie = true;
            flash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            flash.SetActive(false);
            DisableTimer();
        }
    }

    /// <summary>
    /// 스프라이트 반전여부 결정 함수
    /// </summary>
    public void FlipModif()
    {
        if (transform.position.x < GameManager.Instance.Player.transform.position.x)
        {
            spriteRenderer.flipX = true;
            flipXint = 1;
        }
        else
        {
            spriteRenderer.flipX = false;
            flipXint = -1;
        }
    }
}
