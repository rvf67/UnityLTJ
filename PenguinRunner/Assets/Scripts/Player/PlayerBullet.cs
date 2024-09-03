using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : RecycleObject
{
    /// <summary>
    /// �Ѿ��� �̵��ӵ�
    /// </summary>
    public float moveSpeed = 7.0f;

    /// <summary>
    /// �Ѿ��� ����
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
        // �ʴ� moveSpeed �ӵ���, ���� �������� �÷��̾� �÷��� ����Ʈ ���� �������� �̵��ϱ�
        transform.Translate(Time.deltaTime * moveSpeed, 0, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpeed = Mathf.Abs(moveSpeed);
        gameObject.SetActive(false);
    }
}
