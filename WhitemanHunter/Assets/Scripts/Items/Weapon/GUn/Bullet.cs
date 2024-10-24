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
    /// 총알의 속도
    /// </summary>
    public float shotSpeed=50.0f;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected override void OnReset()
    {
        if (GameManager.Instance.Player != null)
        {
            Shot();
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
        if(other.gameObject.tag == "Wall"){
            DisableTimer();
        }
    }

    /// <summary>
    /// 플레이어 앞쪽으로만 발사할 수 있도록 하는 함수
    /// </summary>
    public void Shot()
    {
        rb.velocity = GameManager.Instance.Player.transform.forward  * shotSpeed;
    }

}
