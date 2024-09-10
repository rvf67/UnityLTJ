using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("적 기본 데이터")]
    /// <summary>
    /// 적의 수명
    /// </summary>
    public float lifeTime = 30.0f;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.0f;

    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);
        OnVisualUpdate(Time.deltaTime);
    }

    /// <summary>
    /// 적 기본 초기화 작업(재활용시)
    /// </summary>
    protected override void OnReset()
    {
        DisableTimer(lifeTime);
    }

    /// <summary>
    /// Enemy의 종류별로 이동처리를 하는 함수(기본적으로 왼쪽으로만 이동)
    /// </summary>
    /// <param name="deltaTime">Time.deltaTime</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * -transform.right, Space.World); // 기본 동작은 왼쪽으로 계속 이동하기
    }

    /// <summary>
    /// Enemy 종류별로 비주얼 변경 처리를 하는 함수(빈함수)
    /// </summary>
    /// <param name="deltaTime"></param>
    protected virtual void OnVisualUpdate(float deltaTime) { }



}
