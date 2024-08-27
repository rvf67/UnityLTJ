using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : EnemyBase
{
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
    /// �������� ��ȣ�� ����� ǥ��
    /// </summary>
    const int NONE = 0; 
    const int RUSH = 1; 
    const int BOSSMISSILE = 2; 
    const int SPAWNSPIKE = 3;

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
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();
        player=GameManager.Instance.Player;

        fire1 = transform.GetChild(0).GetChild(0); //�� 4���� �Ѿ� �߻���ġ�� ã��
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
            Factory.Instance.GetBossMissile(fire1.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire2.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire3.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire4.position); //���� �߻� ������ŭ ����
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
