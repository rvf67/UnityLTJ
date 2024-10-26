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
    protected Rigidbody rb;

    /// <summary>
    /// ���� ��ǥ
    /// </summary>
    protected Transform target;

    /// <summary>
    /// ���� ���͸����
    /// </summary>
    protected MeshRenderer[] bodyMeshs;

    protected virtual void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        enemyUndieLayer = LayerMask.NameToLayer("EnemyDie");
    }
    protected virtual void Start()
    {
        target = GameManager.Instance.Player.transform;
        bodyMeshs = GetComponentsInChildren<MeshRenderer>();
    }
    protected override void OnReset()
    {
        if (bodyMeshs != null)
        {
            MeshsChange(bodyMeshs,Color.white);
        }
        gameObject.layer = enemyLayer;
        health = maxHealth;
    }
    protected virtual void OnTriggerEnter(Collider other)
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
        MeshsChange(bodyMeshs, Color.red);
        yield return new WaitForSeconds(0.1f);
        if(health > 0)
        {
            MeshsChange(bodyMeshs, Color.white);
        }
        else
        {
            MeshsChange(bodyMeshs,Color.gray);
            gameObject.layer=enemyUndieLayer;
            Die();
            DisableTimer(0.4f);
        }
    }
    public void MeshsChange(MeshRenderer[] meshRenderers, Color color)
    {
        foreach (MeshRenderer bodyMesh in meshRenderers)
        {
            bodyMesh.material.color = color;
        }
    }
    protected virtual IEnumerator Attack()
    {
        yield return null;
    }
    protected virtual void Die()
    {

    }
}
