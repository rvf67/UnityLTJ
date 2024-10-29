using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// 가까이에 있는 상호작용한 오브젝트
    /// </summary>
    GameObject nearObject;
    /// <summary>
    /// 무적 레이어
    /// </summary>
    int undieLayer;
    /// <summary>
    /// 플레이어 레이어
    /// </summary>
    int playerLayer;
    /// <summary>
    /// 장착하고 있는 무기
    /// </summary>
    public Weapon equipWeapon;

    /// <summary>
    /// 가지고 있는 여분탄약
    /// </summary>
    int ammo;
    /// <summary>
    /// 원거리 무기가 가질 수 있는 최대 탄약
    /// </summary>
    public int ammoMax;

    public int Ammo
    {
        get => ammo;
        set
        {
            if (value != ammo)
            {
                ammo = value;
            }
        }
    }
    /// <summary>
    /// 현재체력
    /// </summary>
    float health;
    /// <summary>
    /// 가질수 있는 최대체력
    /// </summary>
    public float healthMax=100.0f;

    public float Health
    {
        get =>health;
        set
        {
            if (value != health)
            {
                health = value;
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    /// <summary>
    /// 가지고 있는 돈
    /// </summary>
    int coin;
    /// <summary>
    /// 가질수 있는 최대 돈
    /// </summary>
    public int coinMax;

    /// <summary>
    /// 스왑 애니메이터용 해시
    /// </summary>
    readonly int Swap_Hash = Animator.StringToHash("Swap");
    /// <summary>
    /// 플레이어가 가진 무기들
    /// </summary>
    public GameObject[] weapons;
    /// <summary>
    /// 소지하고 있는지에 대한 배열
    /// </summary>
    public bool[] hasWeapons;
    /// <summary>
    /// 스왑중인지 체크하는 변수
    /// </summary>
    public bool isSwap;
    /// <summary>
    /// 무적시간
    /// </summary>
    public float undieTime = 1.0f;
    /// <summary>
    /// 맞았는지 여부
    /// </summary>
    public bool isDamage;
    /// <summary>
    /// 플레이어의 움직임 컴포넌트
    /// </summary>
    PlayerMovement playerMovement;
    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;
    /// <summary>
    /// 플레이어 메시들
    /// </summary>
    MeshRenderer[] meshs;

    private void Awake()
    {
        playerMovement= transform.GetComponent<PlayerMovement>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        meshs = transform.GetComponentsInChildren<MeshRenderer>();
        Health = healthMax;
    }
    private void Start()
    {
        undieLayer = LayerMask.NameToLayer("Undie");
        playerLayer = LayerMask.NameToLayer("Player");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.itemType)
            {
                case Item.ItemType.Ammo:
                    ammo += item.value;
                    if(ammo > ammoMax)
                        ammo = ammoMax;
                    break;
                case Item.ItemType.Coin:
                    coin += item.value;
                    if(coin > coinMax)
                        coin = coinMax;
                    break;
                case Item.ItemType.Heart:
                    health += item.value;
                    if (health > healthMax)
                        health = healthMax;
                    break;
                case Item.ItemType.Grenade:
                    Debug.Log("미구현 차후 구현 예정");
                    break;
            }
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                Health -= enemyBullet.damage;
                StartCoroutine(OnDamage());
            }        
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

    /// <summary>
    /// 어떤 물체와 상호작용했을때 실행할 함수
    /// </summary>
    public void Interact()
    {
        if (nearObject != null && !playerMovement.IsDodge)
        {
            Item item = nearObject.GetComponent<Item>();
            int weaponIndex = item.value;
            hasWeapons[weaponIndex] = true;
            nearObject.SetActive(false);
        }
    }

    /// <summary>
    /// 무기스왑 함수
    /// </summary>
    /// <param name="select"></param>
    public void Swap(int select)
    {
        int weaponIndex = -1;
        if (!playerMovement.IsDodge && hasWeapons[select-1])
        {
            weaponIndex = select-1;
            if(equipWeapon != null)
            {
                if(equipWeapon.gameObject==weapons[weaponIndex])        //장착한 무기가 선택한 무기와 같으면
                {
                    equipWeapon.gameObject.SetActive(false);
                    equipWeapon = null;                                 //무기 해제
                    return;
                }
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            animator.SetTrigger(Swap_Hash);

            isSwap = true;
            Invoke("SwapOut", 0.3f);
        }
    }

    /// <summary>
    /// 무기 스왑 종료 함수
    /// </summary>
    public void SwapOut()
    {
        isSwap = false;
    }

    public void Die()
    {
        Debug.Log("죽다.");
    }

    IEnumerator OnDamage()
    {
        isDamage =true;
        gameObject.layer = undieLayer;
        float timeElapsed = 0.0f;
        while (timeElapsed < undieTime)
        {
            timeElapsed += Time.deltaTime;
            foreach (MeshRenderer mesh in meshs)
            {
                //30.0f는 깜빡이는 속도 증폭율
                float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;
                mesh.material.color = new Color(1,alpha,alpha,1);
            }
            yield return null;
        }
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        isDamage = false;
        gameObject.layer=playerLayer;
    }

}
