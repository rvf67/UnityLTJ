using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : EnemyBase
{
    private int prevPatternRandom = 0;
    /// <summary>
    /// �̻��� ���� �߻綧 �߻纰 ����
    /// </summary>
    public float barrageInterval = 0.2f;
    /// <summary>
    /// �����߻� �� �߻� Ƚ��
    /// </summary>
    public int barrageCount = 3;
    int totalPatternCount;
    private static readonly int NONE = 0; 
    private static readonly int RUSH = 1; 
    private static readonly int PATTERN2 = 2; 
    private static readonly int PATTERN3 = 3; 
    private static readonly int PATTERN4 = 4;

    /// <summary>
    /// �̻��Ϲ߻� ��ġ 1
    /// </summary>
    Transform fire1;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 2
    /// </summary>
    Transform fire2;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 3(�̻���)
    /// </summary>
    Transform fire3;
    /// <summary>
    /// �̻��Ϲ߻� ��ġ 4(�̻���)
    /// </summary>
    Transform fire4;

    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        fire1 = transform.GetChild(0).GetChild(0); //�� 4���� �Ѿ� �߻���ġ�� ã��
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
            Factory.Instance.GetBossMissile(fire3.position); //���� �߻� ������ŭ ����
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
