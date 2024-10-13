using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// 가까이에 있는 상호작용한 오브젝트
    /// </summary>
    GameObject nearObject;
    
    /// <summary>
    /// 장착하고 있는 무기
    /// </summary>
    GameObject equipWeapon;

    /// <summary>
    /// 플레이어의 움직임 컴포넌트
    /// </summary>
    PlayerMovement playerMovement;

    /// <summary>
    /// 플레이어가 가진 무기들
    /// </summary>
    public GameObject[] weapons;
    public bool[] hasWeapons;
    private void Awake()
    {
        playerMovement= transform.GetComponent<PlayerMovement>();
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

    public void Swap(int select)
    {
        int weaponIndex = -1;
        if (!playerMovement.IsDodge && hasWeapons[select-1])
        {
            if(equipWeapon != null) 
                equipWeapon.SetActive(false);
            weaponIndex = select-1;
            equipWeapon = weapons[weaponIndex];
            weapons[weaponIndex].SetActive(true);
        }
    }
}
