using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class EnemyBoss : EnemyBase
{
    /// <summary>
    /// 공 스폰 방향
    /// </summary>
    private Vector2 spawnDIr;
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    /// <summary>
    /// 이전에 사용한 패턴
    /// </summary>
    private int prevPatternRandom = 0;
    /// <summary>
    /// 미사일 일제 발사때 발사별 간경
    /// </summary>
    public float barrageInterval = 0.2f;
    /// <summary>
    /// 일제발사 때 발사 횟수
    /// </summary>
    public int barrageCount = 3;
    /// <summary>
    /// 돌진횟수
    /// </summary>
    public int rushCount = 4;

    /// <summary>
    /// 속도 복사
    /// </summary>
    float rushSpeedCopy;
    /// <summary>
    /// 각패턴의 번호를 상수로 표현
    /// </summary>
    const int NONE = 0; 
    const int RUSH = 1; 
    const int BOSSMISSILE = 2; 
    const int SPAWNSPIKE = 3;

    /// <summary>
    /// 라인그리기용 벡터
    /// </summary>
    private Vector2 newDirection;
    /// <summary>
    /// 플레이어로의 방향
    /// </summary>
    Vector3 direction;
    /// <summary>
    /// 미사일발사 위치 1
    /// </summary>
    Transform fire1;
    /// <summary>
    /// 미사일발사 위치 2
    /// </summary>
    Transform fire2;
    /// <summary>
    /// 미사일발사 위치 3
    /// </summary>
    Transform fire3;
    /// <summary>
    /// 미사일발사 위치 4
    /// </summary>
    Transform fire4;

    Animator animator;

    Rigidbody2D rigid;
    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer spriteRenderer;
    LineRenderer lineRenderer;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        player=GameManager.Instance.Player;

        fire1 = transform.GetChild(0).GetChild(0); //각 4개의 총알 발사위치를 찾음
        fire2 = transform.GetChild(0).GetChild(1);
        fire3 = transform.GetChild(0).GetChild(2);
        fire4 = transform.GetChild(0).GetChild(3);
    }

    private void Start()
    {
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        rushSpeedCopy = rushSpeed;
        RandomPattern();
    }

    /// <summary>
    /// 플레이어에게 연속돌진
    /// </summary>
    /// <returns></returns>
    IEnumerator Rush() 
    {
        yield return new WaitForSeconds(3.0f);
        for(int i=0; i < rushCount; i++)
        {
            rushSpeed = 0;
            Factory.Instance.GetDangerLine(transform.position);
            RushToPlayer(transform.position);
            yield return new WaitForSeconds(1.0f);
            rushSpeed = rushSpeedCopy;
            yield return new WaitForSeconds(2.0f);
        }
        rushSpeed = 0;
        RandomPattern();
    }

    /// <summary>
    /// 플레이어를 추적하는 철갑상어 생성
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotSwordFishMissile() 
    {
        player.DangerMissile();
        yield return new WaitForSeconds(3.0f);
        for (int i = 0; i < barrageCount; i++)
        {
            Factory.Instance.GetBossMissile(fire1.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire2.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire3.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire4.position); //연속 발사 개수만큼 생성
            yield return new WaitForSeconds(barrageInterval);
        }
        RandomPattern();
    }

    /// <summary>
    /// 공 생성
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBall() 
    {
        DangerLineShoot();
        yield return new WaitForSeconds(3.0f);
        DangerLineDelete();
        Factory.Instance.GetBossBall(transform.position,spawnDIr);
        yield return new WaitForSeconds(3.0f);
        RandomPattern();
    }

    /// <summary>
    /// 클리어 화면전환
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadClear()
    {
        animator.SetTrigger("Die");
        Collider2D collider = GetComponent<Collider2D>();
        rigid.velocity = Vector2.zero;
        Destroy(collider);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("ClearScene");
    }
    protected override void Die()
    {
        if (isAlive) // 살아있을 때만 죽을 수 있음
        {
            isAlive = false;            // 죽었다고 표시
            onDie?.Invoke(point);       // 죽었다고 등록된 객체들에게 알리기(등록된 함수 실행)


            OnDie();
        }
    }
    protected override void OnDie()
    {
        GameManager.Instance.AddScore(point);
        StopAllCoroutines();
        StartCoroutine(LoadClear());
    }

    /// <summary>
    /// 이동처리
    /// </summary>
    /// <param name="deltaTime"></param>
    protected override void OnMoveUpdate(float deltaTime)
    {
        rigid.velocity = rushSpeed*direction;
    }

    /// <summary>
    /// 패턴을 랜덤으로 뽑아주는 함수
    /// </summary>
    void RandomPattern()
    {
        int newPattern = Random.Range(1, 4);
        while (newPattern==prevPatternRandom)
        {
            newPattern = Random.Range(1, 4);
        }
        prevPatternRandom=newPattern;
        switch (newPattern)
        {
            case RUSH:
                StartCoroutine(Rush());
                break;
            case BOSSMISSILE:
                StartCoroutine(ShotSwordFishMissile());
                break;
            case SPAWNSPIKE:
                StartCoroutine(SpawnBall());
                break;
        }
    }


    /// <summary>
    /// 경고선 그리는 함수
    /// </summary>
    public void DangerLineShoot()
    {
        Vector2 newDirection = new Vector2(player.transform.position.x-transform.position.x>0 ? 1.0f:-1.0f, 
            player.transform.position.y - transform.position.y > 0 ? 1.0f : -1.0f);
        spawnDIr = newDirection;
        Vector2 newPos = transform.position;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        for (int i = 1; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(newPos+(0.0001f*newDirection), newDirection, Mathf.Infinity, LayerMask.GetMask("BossArea"));
            if (hit.collider != null)
            {
                newPos = hit.point;
                newDirection = Vector2.Reflect(newDirection, hit.normal);
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i, newPos);
            }
            else
            {
                // 만약 충돌이 없으면, 그냥 계속 직선으로 그리기
                newPos += newDirection * 100; // 임의의 큰 값을 사용하여 선을 계속 연장
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i, newPos);
                break;
            }
        }
    }

    /// <summary>
    /// 그렸던 라인을 삭제하는 함수
    /// </summary>
    private void DangerLineDelete()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i,Vector2.zero);
        }
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// 플레이어에게로의 방향계산
    /// </summary>
    /// <param name="pos">자신의 현재위치</param>
    public void RushToPlayer(Vector3 pos)
    {
        direction = (player.transform.position - pos).normalized;
        
        spriteRenderer.flipX = direction.x > 0;
    }
}
