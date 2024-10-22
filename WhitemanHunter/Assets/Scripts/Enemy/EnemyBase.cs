using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /// <summary>
    /// 적 최대체력
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// 적 현재 체력
    /// </summary>
    public int health;

    /// <summary>
    /// 적의 리지드바디
    /// </summary>
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
