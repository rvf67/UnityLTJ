using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    /// <summary>
    /// 적 최대체력
    /// </summary>
    public float maxHealth;
    /// <summary>
    /// 적 현재 체력
    /// </summary>
    public float health;

    /// <summary>
    /// 이 적을 잡았을 때 얻을 점수변수
    /// </summary>
    public int score;
    /// <summary>
    /// 적이 갖고 있는 코인 배열
    /// </summary>
    public GameObject[] coins;

    /// <summary>
    /// 무적레이어
    /// </summary>
    protected int enemyUndieLayer;
    /// <summary>
    /// 적의 리지드바디
    /// </summary>
    protected Rigidbody rb;

    /// <summary>
    /// 적의 목표
    /// </summary>
    protected Transform target;

    /// <summary>
    /// 적의 머터리얼들
    /// </summary>
    protected MeshRenderer[] bodyMeshs;

    /// <summary>
    /// 생존한 적 레이어
    /// </summary>
    int enemyLayer;
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

    /// <summary>
    /// 데미지 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator OnDamage()
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
    /// <summary>
    /// 메시들의 색을 한번에 바꿔줄 함수
    /// </summary>
    /// <param name="meshRenderers">메시들</param>
    /// <param name="color">색깔</param>
    public void MeshsChange(MeshRenderer[] meshRenderers, Color color)
    {
        foreach (MeshRenderer bodyMesh in meshRenderers)
        {
            bodyMesh.material.color = color;
        }
    }
    /// <summary>
    /// 공격했을 때 실행할 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Attack()
    {
        yield return null;
    }
    /// <summary>
    /// 죽었을 때 실행하는 함수
    /// </summary>
    protected virtual void Die()
    {
        PlayerInteraction targeInteraction=target.transform.GetComponent<PlayerInteraction>();
        targeInteraction.Score += score;
    }
}
