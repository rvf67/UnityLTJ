using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    // ��������Ʈ - �Լ��� ������ �� �ִ� ������ Ÿ��. �� ���뵵 = Ư�� ��Ȳ���� �˸��� ���� ���.
    // Action : �Ķ���Ϳ� ������ ���� �Լ��� ���� ������ ��������Ʈ
    // Func<T> : ������ �ݵ�� �ְ� �Ķ���͵� ���� ������ ��������Ʈ

    /// <summary>
    /// ��Ȱ�� ������Ʈ�� ��Ȱ��ȭ �� �� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action onDisable = null;

    protected virtual void OnEnable()
    {
        StopAllCoroutines();    // ������ ���� ���̴� �ڷ�ƾ ��� ����
    }

    protected virtual void OnDisable()
    {
        onDisable?.Invoke();    // onDisable�� null�� �ƴϸ� �����϶�        
    }

    /// <summary>
    /// ��Ȱ�� �� �� �ʱ�ȭ ��Ű�� �Լ�(���Լ�)
    /// </summary>
    protected virtual void OnReset()
    {
    }

    /// <summary>
    /// ���� �ð� �Ŀ� ���ӿ�����Ʈ�� �ڵ����� ��Ȱ��ȭ��Ű�� �Լ�
    /// </summary>
    /// <param name="time">��ٸ� �ð�</param>
    protected void DisableTimer(float time = 0.0f)
    {
        StartCoroutine(LifeOver(time));
    }

    IEnumerator LifeOver(float time = 0.0f)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
