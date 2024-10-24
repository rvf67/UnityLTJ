using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���������Ʈ�� �÷��̾��� �����տ� �̸� ���������
/// </summary>
public class Weapon : MonoBehaviour
{
    /// <summary>
    /// ���� Ÿ��
    /// </summary>
    public enum WeaponType
    {
        Melee,
        Range
    }
    /// <summary>
    /// ����Ÿ�� ����
    /// </summary>
    public WeaponType type;
    /// <summary>
    /// �����϶� ������
    /// </summary>
    public float damage;
    /// <summary>
    /// ���Ÿ��϶� ���ݼӵ�
    /// </summary>
    public float rate;
    /// <summary>
    /// ������ �ִ� �ִ� źâ
    /// </summary>
    public int maxAmmo;
    /// <summary>
    /// ���� ź��
    /// </summary>
    public int currentAmmo;
    /// <summary>
    /// �������ݽ� ���� ����
    /// </summary>
    public BoxCollider meleeArea;
    /// <summary>
    /// Ʈ���� ����Ʈ
    /// </summary>
    public TrailRenderer trailEffect;
    public enum RangeType
    {
        None =0,
        HandGun,
        SubMachineGun,
    }
    /// <summary>
    /// ���Ÿ��� �� Ÿ��
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

    /// <summary>
    /// ���� ��� �Լ�
    /// </summary>
    public void Use()
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine(ActivateMelee());
            StartCoroutine(ActivateMelee());
        }
        else if(type == WeaponType.Range && currentAmmo > 0)
        {
            currentAmmo--;
            StartCoroutine(ActivateShot());
        }
    }


    /// <summary>
    /// �������� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// ���Ÿ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator ActivateShot()
    {
        //�Ѿ˻���
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
            
        //ź�� ����
        Factory.Instance.GetBulletCase(bulletCasePostion.position,Vector3.up*10);
        yield return null;   
    }
}
