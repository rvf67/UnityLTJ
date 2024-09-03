using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBase : RecycleObject
{
    [Header("적 기본 데이터")]

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float rushSpeed = 3.0f;

    /// <summary>
    /// 이 적을 죽였을 때 얻는 점수
    /// </summary>
    public int point = 10;

    /// <summary>
    /// 적의 최대 HP
    /// </summary>
    public int maxHP=1;

    /// <summary>
    /// 적의 HP
    /// </summary>
    int hp=1;

    /// <summary>
    /// 생존 여부를 표현하는 변수
    /// </summary>
    protected bool isAlive = true;

    /// <summary>
    /// 적의 HP를 get/set할 수 있는 프로퍼티
    /// </summary>
    public int HP
    {
        get => hp;          // 읽기는 public
        private set         // 쓰기는 private
        {
            hp = value;
            if (hp < 1)      // 0이되면
            {
                Die();    // 사망 처리 수행
            }
        }
    }

    /// <summary>
    /// 자신이 죽었음을 알리는 델리게이트(int : 자신의 점수)
    /// </summary>
    public Action<int> onDie;
    protected virtual void Awake()
    {
        HP = maxHP;
        isAlive = true;
    }
    protected virtual void Update()
    {
        OnMoveUpdate(Time.deltaTime);
        OnVisualUpdate(Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            HP--;   // 부딪칠 때마다 HP감소(적끼리는 부딪치지 않는다)
        }
    }

    
    /// <summary>
    /// 적 기본 초기화 작업(재활용시)
    /// </summary>
    protected override void OnReset()
    {
        HP = maxHP;
        isAlive = true;
    }

    /// <summary>
    /// Enemy의 종류별로 이동처리를 하는 함수(기본적으로 왼쪽으로만 이동)
    /// </summary>
    /// <param name="deltaTime">Time.deltaTime</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * rushSpeed * -transform.right, Space.World); // 기본 동작은 왼쪽으로 계속 이동하기
    }

    /// <summary>
    /// Enemy 종류별로 비주얼 변경 처리를 하는 함수(빈함수)
    /// </summary>
    /// <param name="deltaTime"></param>
    protected virtual void OnVisualUpdate(float deltaTime) { }

    /// <summary>
    /// 적이 터질 때 실행될 함수
    /// </summary>
    protected virtual void Die()
    {
        if (isAlive) // 살아있을 때만 죽을 수 있음
        {
            isAlive = false;            // 죽었다고 표시
            onDie?.Invoke(point);       // 죽었다고 등록된 객체들에게 알리기(등록된 함수 실행)


            OnDie();

            DisableTimer();     // 자신을 비활성화 시키기
        }
    }

    /// <summary>
    /// 죽었을 때 적의 종류별로 실행해야 할 일을 수행하는 함수(빈함수)
    /// </summary>
    protected virtual void OnDie()
    {
    }
}
