using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// 총알의 데미지
    /// </summary>
    public int damage;

    /// <summary>
    /// 방향
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// 총알의 속도
    /// </summary>
    public float shotSpeed=50.0f;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// 적의 근접공격 범위인지 확인
    /// </summary>
    public bool isMelee;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected override void OnReset()
    {
        if (GameManager.Instance.Player != null)
        {
            DisableTimer(3.0f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")       // 탄피일 경우 처리
        {
            DisableTimer(0.3f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isMelee&&other.gameObject.tag == "Wall"){
            DisableTimer();
        }
    }

    private void Update()
    {
        if(direction != Vector3.zero)
            rb.velocity = direction  * shotSpeed;
    }

    /// <summary>
    /// 방향을 정해주는 함수
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
        transform.rotation=Quaternion.LookRotation(direction, Vector3.up);
    }
}
