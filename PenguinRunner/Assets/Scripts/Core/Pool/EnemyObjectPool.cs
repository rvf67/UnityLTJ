using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool<T> : ObjectPool<T> where T : EnemyBase
{
    /// <summary>
    /// 적이 하나 생성될 때 실행되는 함수
    /// </summary>
    /// <param name="comp">생성된 적의 컴포넌트</param>
    protected override void OnGenerateObject(T comp)
    {
        comp.onDie += GameManager.Instance.AddScore;   // 사망 델리게이트에 점수 증가 함수를 등록
    }
}
