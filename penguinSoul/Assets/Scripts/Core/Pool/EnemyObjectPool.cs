using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool<T> : ObjectPool<T> where T : EnemyBase
{
    /// <summary>
    /// ���� �ϳ� ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="comp">������ ���� ������Ʈ</param>
    protected override void OnGenerateObject(T comp)
    {
        comp.onDie += GameManager.Instance.AddScore;   // ��� ��������Ʈ�� ���� ���� �Լ��� ���
    }
}
