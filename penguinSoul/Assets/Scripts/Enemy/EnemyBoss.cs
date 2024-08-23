using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : EnemyBase
{
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
    /// 각패턴의 번호를 상수로 표현
    /// </summary>
    const int NONE = 0; 
    const int RUSH = 1; 
    const int BOSSMISSILE = 2; 
    const int SPAWNSPIKE = 3;

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

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

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
        Debug.Log("패턴1");
        yield return new WaitForSeconds(3.0f);
        RandomPattern();
    }
    IEnumerator shotSwordFishMissile() 
    {
        for (int i = 0; i < barrageCount; i++)
        {
            Factory.Instance.GetBossMissile(fire1.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire2.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire3.position); //연속 발사 개수만큼 생성
            Factory.Instance.GetBossMissile(fire4.position); //연속 발사 개수만큼 생성
            yield return new WaitForSeconds(barrageInterval);
        }
        yield return new WaitForSeconds(3.0f);
        RandomPattern();
    }
    IEnumerator Pattern3() 
    {
        Debug.Log("패턴3");
        yield return new WaitForSeconds(3.0f);
        RandomPattern();
    }
    IEnumerator LoadClear()
    {
        animator.SetTrigger("Die");
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
        StartCoroutine(LoadClear());
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
                StartCoroutine(shotSwordFishMissile());
                break;
            case SPAWNSPIKE:
                StartCoroutine(Pattern3());
                break;
        }
    }
}
