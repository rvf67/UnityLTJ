using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    /// <summary>
    /// �� �ִ�ü��
    /// </summary>
    public float maxHealth;
    /// <summary>
    /// �� ���� ü��
    /// </summary>
    public float health;

    /// <summary>
    /// �������̾�
    /// </summary>
    int enemyUndieLayer;
    /// <summary>
    /// ������ �� ���̾�
    /// </summary>
    int enemyLayer;
    /// <summary>
    /// ���� ������ٵ�
    /// </summary>
    Rigidbody rb;

    /// <summary>
    /// ���� ���͸����
    /// </summary>
    protected Material bodyMaterial;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bodyMaterial = transform.GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyUndieLayer = LayerMask.NameToLayer("EnemyDie");
    }
    protected override void OnReset()
    {
        gameObject.layer = enemyLayer;
        health = maxHealth;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (weapon != null)
            {
                health -= weapon.damage;
                StartCoroutine(OnDamage());
            }
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                health -= bullet.damage;
                StartCoroutine(OnDamage());
            }
        }
    }
    IEnumerator OnDamage()
    {
        bodyMaterial.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if(health > 0)
        {
            bodyMaterial.color = Color.white;
        }
        else
        {
            bodyMaterial.color = Color.gray;
            gameObject.layer=enemyUndieLayer;
            DisableTimer(0.4f);
        }
    }
}
