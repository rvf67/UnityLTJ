using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("�� �⺻ ������")]
    /// <summary>
    /// ���� ����
    /// </summary>
    public float lifeTime = 30.0f;

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    public float moveSpeed = 3.0f;

    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);
        OnVisualUpdate(Time.deltaTime);
    }

    /// <summary>
    /// �� �⺻ �ʱ�ȭ �۾�(��Ȱ���)
    /// </summary>
    protected override void OnReset()
    {
        DisableTimer(lifeTime);
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



}
