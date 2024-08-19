using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ������")]

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    public float moveSpeed = 3.0f;

    /// <summary>
    /// �� ���� �׿��� �� ��� ����
    /// </summary>
    public int point = 10;

    /// <summary>
    /// ���� �ִ� HP
    /// </summary>
    public int maxHP=1;

    /// <summary>
    /// ���� HP
    /// </summary>
    int hp=1;

    /// <summary>
    /// ���� ���θ� ǥ���ϴ� ����
    /// </summary>
    protected bool isAlive = true;

    /// <summary>
    /// ���� HP�� get/set�� �� �ִ� ������Ƽ
    /// </summary>
    public int HP
    {
        get => hp;          // �б�� public
        private set         // ����� private
        {
            hp = value;
            if (hp < 1)      // 0�̵Ǹ�
            {
                Die();    // ��� ó�� ����
            }
        }
    }

    /// <summary>
    /// �ڽ��� �׾����� �˸��� ��������Ʈ(int : �ڽ��� ����)
    /// </summary>
    public Action<int> onDie;
    protected virtual void Awake()
    {
        HP = maxHP;
        isAlive = true;
    }
    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);
        OnVisualUpdate(Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        HP--;   // �ε�ĥ ������ HP����(�������� �ε�ġ�� �ʴ´�)
    }

    
    /// <summary>
    /// �� �⺻ �ʱ�ȭ �۾�(��Ȱ���)
    /// </summary>
    protected override void OnReset()
    {
        HP = maxHP;
        isAlive = true;
    }

    /// <summary>
    /// Enemy�� �������� �̵�ó���� �ϴ� �Լ�(�⺻������ �������θ� �̵�)
    /// </summary>
    /// <param name="deltaTime">Time.deltaTime</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * -transform.right, Space.World); // �⺻ ������ �������� ��� �̵��ϱ�
    }

    /// <summary>
    /// Enemy �������� ���־� ���� ó���� �ϴ� �Լ�(���Լ�)
    /// </summary>
    /// <param name="deltaTime"></param>
    protected virtual void OnVisualUpdate(float deltaTime) { }

    /// <summary>
    /// ���� ���� �� ����� �Լ�
    /// </summary>
    protected virtual void Die()
    {
        if (isAlive) // ������� ���� ���� �� ����
        {
            isAlive = false;            // �׾��ٰ� ǥ��
            onDie?.Invoke(point);       // �׾��ٰ� ��ϵ� ��ü�鿡�� �˸���(��ϵ� �Լ� ����)


            OnDie();

            DisableTimer();     // �ڽ��� ��Ȱ��ȭ ��Ű��
        }
    }

    /// <summary>
    /// �׾��� �� ���� �������� �����ؾ� �� ���� �����ϴ� �Լ�(���Լ�)
    /// </summary>
    protected virtual void OnDie()
    {
    }
}
