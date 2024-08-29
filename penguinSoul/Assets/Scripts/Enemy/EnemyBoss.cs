using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class EnemyBoss : EnemyBase
{
    /// <summary>
    /// �� ���� ����
    /// </summary>
    private Vector2 spawnDIr;
    /// <summary>
    /// �÷��̾�
    /// </summary>
    Player player;
    /// <summary>
    /// ������ ����� ����
    /// </summary>
    private int prevPatternRandom = 0;
    /// <summary>
    /// �̻��� ���� �߻綧 �߻纰 ����
    /// </summary>
    public float barrageInterval = 0.2f;
    /// <summary>
    /// �����߻� �� �߻� Ƚ��
    /// </summary>
    public int barrageCount = 3;
    /// <summary>
    /// ����Ƚ��
    /// </summary>
    public int rushCount = 4;

    /// <summary>
    /// �ӵ� ����
    /// </summary>
    float rushSpeedCopy;
    /// <summary>
    /// �������� ��ȣ�� ����� ǥ��
    /// </summary>
    const int NONE = 0; 
    const int RUSH = 1; 
    const int BOSSMISSILE = 2; 
    const int SPAWNSPIKE = 3;

    /// <summary>
    /// ���α׸���� ����
    /// </summary>
    private Vector2 newDirection;
    /// <summary>
    /// �÷��̾���� ����
    /// </summary>
    Vector3 direction;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 1
    /// </summary>
    Transform fire1;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 2
    /// </summary>
    Transform fire2;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 3
    /// </summary>
    Transform fire3;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 4
    /// </summary>
    Transform fire4;

    Animator animator;

    Rigidbody2D rigid;
    /// <summary>
    /// ��������Ʈ ������
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

        fire1 = transform.GetChild(0).GetChild(0); //�� 4���� �Ѿ� �߻���ġ�� ã��
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
    /// �÷��̾�� ���ӵ���
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
    /// �÷��̾ �����ϴ� ö����� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotSwordFishMissile() 
    {
        player.DangerMissile();
        yield return new WaitForSeconds(3.0f);
        for (int i = 0; i < barrageCount; i++)
        {
            Factory.Instance.GetBossMissile(fire1.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire2.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire3.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire4.position); //���� �߻� ������ŭ ����
            yield return new WaitForSeconds(barrageInterval);
        }
        RandomPattern();
    }

    /// <summary>
    /// �� ����
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
    /// Ŭ���� ȭ����ȯ
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
        if (isAlive) // ������� ���� ���� �� ����
        {
            isAlive = false;            // �׾��ٰ� ǥ��
            onDie?.Invoke(point);       // �׾��ٰ� ��ϵ� ��ü�鿡�� �˸���(��ϵ� �Լ� ����)


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
    /// �̵�ó��
    /// </summary>
    /// <param name="deltaTime"></param>
    protected override void OnMoveUpdate(float deltaTime)
    {
        rigid.velocity = rushSpeed*direction;
    }

    /// <summary>
    /// ������ �������� �̾��ִ� �Լ�
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
    /// ��� �׸��� �Լ�
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
                // ���� �浹�� ������, �׳� ��� �������� �׸���
                newPos += newDirection * 100; // ������ ū ���� ����Ͽ� ���� ��� ����
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i, newPos);
                break;
            }
        }
    }

    /// <summary>
    /// �׷ȴ� ������ �����ϴ� �Լ�
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
    /// �÷��̾�Է��� ������
    /// </summary>
    /// <param name="pos">�ڽ��� ������ġ</param>
    public void RushToPlayer(Vector3 pos)
    {
        direction = (player.transform.position - pos).normalized;
        
        spriteRenderer.flipX = direction.x > 0;
    }
}
