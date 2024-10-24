using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// �Ѿ��� ������
    /// </summary>
    public int damage;


    /// <summary>
    /// �Ѿ��� �ӵ�
    /// </summary>
    public float shotSpeed=50.0f;

    /// <summary>
    /// ������ٵ�
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
        if (collision.gameObject.tag == "Floor")       // ź���� ��� ó��
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
    /// �÷��̾� �������θ� �߻��� �� �ֵ��� �ϴ� �Լ�
    /// </summary>
    public void Shot()
    {
        rb.velocity = GameManager.Instance.Player.transform.forward  * shotSpeed;
    }

}
