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
    /// ����
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// �Ѿ��� �ӵ�
    /// </summary>
    public float shotSpeed=50.0f;

    /// <summary>
    /// ������ٵ�
    /// </summary>
    protected Rigidbody rb;

    /// <summary>
    /// ���� �������� �������� Ȯ��
    /// </summary>
    public bool isMelee;
    /// <summary>
    /// ���������� Ȯ���ϴ� ����
    /// </summary>
    public bool isRock;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }


    protected override void OnReset()
    {
        if (!isMelee&&GameManager.Instance.Player != null)
        {
            DisableTimer(7.0f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isRock&&collision.gameObject.tag == "Floor")       // ź���� ��� ó��
        {
            DisableTimer(0.3f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isMelee&&other.gameObject.tag == "Wall"){
            DisableTimer(0.3f);
        }
    }

    private void Update()
    {
        if(direction != Vector3.zero && !isRock)
            rb.velocity = direction  * shotSpeed;
    }

    /// <summary>
    /// ������ �����ִ� �Լ�
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
        transform.rotation=Quaternion.LookRotation(direction, Vector3.up);
    }
}
