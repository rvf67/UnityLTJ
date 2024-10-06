using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    /// <summary>
    /// 공격 애니메이션 재생시간(공통)
    /// </summary>
    const float AttackAnimLenght = 0.533f;

    /// <summary>
    /// 쿨타임 설정용 변수(콤보ㅗ를 위해서 애니 시간보다 작아야한다.)
    /// </summary>
    public float maxCoolTime = 0.3f;
    
    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float coolTime =0.0f;
    Animator animator;

    readonly int Attack_Hash = Animator.StringToHash("Attack");
    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>(); 
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
        Attack();
    }
    /// <summary>
    /// 공격 한번을 하는 함수
    /// </summary>
    void Attack()
    {
        if (coolTime < 0)
        {
            animator.SetTrigger(Attack_Hash);
            coolTime = maxCoolTime;
        }

    }
}
