using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 무가 타입
    /// </summary>
    public enum WeaponType
    {
        Melee,
        Range
    }
    /// <summary>
    /// 무기타입 접근
    /// </summary>
    public WeaponType type;
    /// <summary>
    /// 근접일때 데미지
    /// </summary>
    public float damage;
    /// <summary>
    /// 원거리일때 공격속도
    /// </summary>
    public float rate;
    /// <summary>
    /// 근접공격시 근접 범위
    /// </summary>
    public BoxCollider meleeArea;
    /// <summary>
    /// 트레일 이펙트
    /// </summary>
    public TrailRenderer trailEffect;
    public enum RangeType
    {
        None =0,
        HandGun,
        SubMachineGun,
    }
    /// <summary>
    /// 원거리일 때 타입
    /// </summary>
    public RangeType rangeType;
    Transform firePosition;
    Transform bulletCasePostion;
    private void Awake()
    {
        bulletCasePostion = transform.GetChild(1);
    }
    private void Start()
    {
        firePosition = GameManager.Instance.Player.transform.GetChild(1);
    }

    public void Use()
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine(ActivateMelee());
            StartCoroutine(ActivateMelee());
        }
        else if(type == WeaponType.Range)
        {
            StopCoroutine(ActivateShot());
            StartCoroutine(ActivateShot());
        }
    }
    IEnumerator ActivateMelee()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;    
    }

    IEnumerator ActivateShot()
    {
        //총알생성
        switch (rangeType)
        {
            case RangeType.None:
                break;
            case RangeType.HandGun:
                Factory.Instance.GetHandBullet(firePosition.position);
                break;
            case RangeType.SubMachineGun:
                Factory.Instance.GetSubBullet(firePosition.position);
                break;

        }
            
        //탄피 생성
        Factory.Instance.GetBulletCase(bulletCasePostion.position,Vector3.up*10);
        yield return null;   
    }
}
