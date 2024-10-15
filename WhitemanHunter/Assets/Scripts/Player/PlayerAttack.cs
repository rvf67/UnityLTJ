using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    PlayerInteraction playerInteraction;

    /// <summary>
    /// 쿨타임 설정용 변수(콤보를 위해서 애니 시간보다 작아야한다.)
    /// </summary>
    public float maxCoolTime = 2.0f;
    
    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float coolTime =0.0f;
    Animator animator;
    /// <summary>
    /// 애니메이션용 스윙 해시
    /// </summary>
    readonly int Swing_Hash = Animator.StringToHash("Swing");
    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>(); 
        playerInteraction = GetComponent<PlayerInteraction>();
    }
    private void Start()
    {
        coolTime = maxCoolTime;
    }
    private void Update()
    {
        coolTime -=Time.deltaTime;
    }
    /// <summary>
    /// 공격입력이 들어오면 실행되는 함수
    /// </summary>
    public void OnAttackInput()
    {
        if(playerInteraction.equipWeapon != null)   //장착한 무기가 있을 때만
        {
            Attack(playerInteraction.equipWeapon);
        }
    }
    /// <summary>
    /// 공격 한번을 하는 함수
    /// </summary>
    void Attack(Weapon equip)
    {
        if (coolTime < 0)
        {
            equip.Use();
            if (equip.type == Weapon.WeaponType.Melee)
            {
                animator.SetTrigger(Swing_Hash);
            }
            coolTime = maxCoolTime;
        }

    }
}
