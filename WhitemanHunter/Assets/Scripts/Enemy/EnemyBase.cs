using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /// <summary>
    /// �� �ִ�ü��
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// �� ���� ü��
    /// </summary>
    public int health;

    /// <summary>
    /// ���� ������ٵ�
    /// </summary>
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
