using System.Collections;
using System.Collections.Generic;
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
    int totalPatternCount;
    private static readonly int NONE = 0; 
    private static readonly int RUSH = 1; 
    private static readonly int PATTERN2 = 2; 
    private static readonly int PATTERN3 = 3; 
    private static readonly int PATTERN4 = 4;

    /// <summary>
    /// 미사일발사 위치 1
    /// </summary>
    Transform fire1;
    /// <summary>
    /// 미사일발사 위치 2
    /// </summary>
    Transform fire2;
    /// <summary>
    /// 미사일발사 위치 3(미사일)
    /// </summary>
    Transform fire3;
    /// <summary>
    /// 미사일발사 위치 4(미사일)
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

    
    IEnumerator Rush() 
    {
        
        yield return null; 
    }
    IEnumerator shotSwordFishMissile() 
    {
        for (int i = 0; i < barrageCount; i++)
        {
            Factory.Instance.GetBossMissile(fire3.position); //연속 발사 개수만큼 생성
            yield return new WaitForSeconds(barrageInterval);
        }
        yield return null;
    }
    IEnumerator Pattern3() { yield return null; }
    IEnumerator Pattern4() { yield return null; }
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
        int newPattern = Random.Range(1, 5);
        while (newPattern==prevPatternRandom)
        {
            newPattern = Random.Range(1, 5);
        }
        prevPatternRandom=newPattern;
        switch (newPattern)
        {
            case 1:
                StartCoroutine(Rush());
                break;
            case 2:
                StartCoroutine(shotSwordFishMissile());
                break;
            case 3:
                StartCoroutine(Pattern3());
                break;
            case 4:
                StartCoroutine(Pattern4());
                break;
        }
    }
}
