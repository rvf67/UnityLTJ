using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : EnemyBase
{
    private int patternRandom = 0;
    int totalPatternCount;
    private static readonly int NONE = 0; 
    private static readonly int PATTERN1 = 1; 
    private static readonly int PATTERN2 = 2; 
    private static readonly int PATTERN3 = 3; 
    private static readonly int PATTERN4 = 4;

    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    IEnumerator RandomPattern()
    {
        switch (patternRandom)
        {
            case 1:
                StartCoroutine(Pattern1());
                break;
            case 2:
                StartCoroutine(Pattern2());
                break;
            case 3:
                StartCoroutine(Pattern3());
                break;
            case 4:
                StartCoroutine(Pattern4());
                break;
        }
        yield return null;
    }
    IEnumerator Pattern1() { yield return null; }
    IEnumerator Pattern2() { yield return null; }
    IEnumerator Pattern3() { yield return null; }
    IEnumerator Pattern4() { yield return null; }

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
    IEnumerator LoadClear()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("ClearScene");
    }
}
