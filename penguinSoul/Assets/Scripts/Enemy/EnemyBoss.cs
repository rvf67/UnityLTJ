using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    /// <summary>
    /// �������� ��ȣ�� ����� ǥ��
    /// </summary>
    const int NONE = 0; 
    const int RUSH = 1; 
    const int BOSSMISSILE = 2; 
    const int SPAWNSPIKE = 3;

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

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

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
        Debug.Log("����1");
        yield return new WaitForSeconds(3.0f);
        RandomPattern();
    }
    IEnumerator shotSwordFishMissile() 
    {
        for (int i = 0; i < barrageCount; i++)
        {
            Factory.Instance.GetBossMissile(fire1.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire2.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire3.position); //���� �߻� ������ŭ ����
            Factory.Instance.GetBossMissile(fire4.position); //���� �߻� ������ŭ ����
            yield return new WaitForSeconds(barrageInterval);
        }
        yield return new WaitForSeconds(3.0f);
        RandomPattern();
    }
    IEnumerator Pattern3() 
    {
        Debug.Log("����3");
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
