using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : EnemyBase
{
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
    /// 각패턴의 번호를 상수로 표현
    /// </summary>
    const int NONE = 0; 
    const int RUSH = 1; 
    const int BOSSMISSILE = 2; 
    const int SPAWNSPIKE = 3;

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
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();
        player=GameManager.Instance.Player;

        fire1 = transform.GetChild(0).GetChild(0); //각 4개의 총알 발사위치를 찾음
        fire2 = transform.GetChild(0).GetChild(1);
        fire3 = transform.GetChild(0).GetChild(2);
        fire4 = transform.GetChild(0).GetChild(3);
    }

    private void Start()
    {
        RandomPattern();
    }


    IEnumerator Rush() 
    {
        for(int i=0; i < rushCount; i++)
        {
            Factory.Instance.GetDangerLine(transform.position);
            rushSpeed = 10.0f;
            RushToPlayer();
            yield return new WaitForSeconds(2.0f);
        }
        rushSpeed = 0;
        yield return new WaitForSeconds(7.0f);
        RandomPattern();
    }
    IEnumerator ShotSwordFishMissile() 
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < barrageCount; i++)
        {
            Factory.Instance.GetBossMissile(fire1.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire2.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire3.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire4.position); //연속 발사 개수만큼 생성
            yield return new WaitForSeconds(barrageInterval);
        }
        yield return new WaitForSeconds(5.0f);
        RandomPattern();
    }
    IEnumerator SpawnBall() 
    {
        yield return new WaitForSeconds(2.0f);
        Factory.Instance.GetBossBall(transform.position);
        yield return new WaitForSeconds(5.0f);
        RandomPattern();
    }
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

    protected override void OnMoveUpdate(float deltaTime)
    {
        rigid.velocity = rushSpeed*direction;
    }
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

    public void RushToPlayer()
    {
        direction = (player.transform.position - transform.position).normalized;
        
        spriteRenderer.flipX = direction.x > 0;
    }
}
