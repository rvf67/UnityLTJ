using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    /// <summary>
    /// 플레이어 물체 상호작용
    /// </summary>
    PlayerInteraction playerInteraction;
    /// <summary>
    /// 플레이어 움직임
    /// </summary>
    PlayerMovement playerMovement;
    /// <summary>
    /// 쿨타임 설정용 변수(콤보를 위해서 애니 시간보다 작아야한다.)
    /// </summary>
    public float maxCoolTime = 2.0f;
    /// <summary>
    /// 공격여부
    /// </summary>
    public bool isAttack;
    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float coolTime =0.0f;
    Animator animator;
    /// <summary>
    /// 애니메이션용 스윙 해시
    /// </summary>
    readonly int Swing_Hash = Animator.StringToHash("Swing");
    /// <summary>
    /// 애니메이션용 샷 해시
    /// </summary>
    readonly int Shot_Hash = Animator.StringToHash("Shot");
    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>(); 
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        coolTime +=Time.deltaTime;
    }
    /// <summary>
    /// 공격입력이 들어오면 실행되는 함수
    /// </summary>
    public void OnAttackInput()
    { 
        if(playerInteraction.equipWeapon != null && !playerInteraction.isSwap && !playerMovement.IsDodge)   //장착한 무기가 있을 때만,스왑중이 아닐때, 회피중이 아닐때
        {
            isAttack = true;
            StartCoroutine(Attack(playerInteraction.equipWeapon));
        }
    }
    /// <summary>
    /// 공격을 끝내기 위한 함수
    /// </summary>
    public void OnAttackEndInput()
    {
        if (playerInteraction.equipWeapon != null && !playerInteraction.isSwap && !playerMovement.IsDodge)   //장착한 무기가 있을 때만,스왑중이 아닐때, 회피중이 아닐때
        {
            isAttack = false;
        }
    }
    /// <summary>
    /// 연사함수
    /// </summary>
    /// <param name="equip">장착중인 무기</param>
    /// <returns></returns>
    IEnumerator Attack(Weapon equip)
    {
        if (coolTime > equip.rate)
        {
            while (isAttack)
            {
                equip.Use();

                animator.SetTrigger(equip.type == Weapon.WeaponType.Melee ? Swing_Hash : Shot_Hash);

                yield return new WaitForSeconds(equip.rate); 
            }
            coolTime = 0;
        }
    }

}
