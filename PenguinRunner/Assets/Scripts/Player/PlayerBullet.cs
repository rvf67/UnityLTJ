using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : RecycleObject
{
    /// <summary>
    /// 총알의 이동속도
    /// </summary>
    public float moveSpeed = 7.0f;

    /// <summary>
    /// 총알의 수명
    /// </summary>
    public float lifeTime = 10.0f;

    Rigidbody2D rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        DisableTimer(lifeTime);
    }

    private void Update()
    {
        // 초당 moveSpeed 속도로, 로컬 기준으로 플레이어 플래시 이펙트 기준 방향으로 이동하기
        transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpeed = Mathf.Abs(moveSpeed);
        gameObject.SetActive(false);
    }
}
