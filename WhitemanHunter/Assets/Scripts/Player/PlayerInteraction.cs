using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// �����̿� �ִ� ��ȣ�ۿ��� ������Ʈ
    /// </summary>
    GameObject nearObject;
    
    /// <summary>
    /// �����ϰ� �ִ� ����
    /// </summary>
    public Weapon equipWeapon;

    /// <summary>
    /// �÷��̾��� ������ ������Ʈ
    /// </summary>
    PlayerMovement playerMovement;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator animator;
    /// <summary>
    /// ���� �ִϸ����Ϳ� �ؽ�
    /// </summary>
    readonly int Swap_Hash = Animator.StringToHash("Swap");
    /// <summary>
    /// �÷��̾ ���� �����
    /// </summary>
    public GameObject[] weapons;
    public bool[] hasWeapons;
    private void Awake()
    {
        playerMovement= transform.GetComponent<PlayerMovement>();
        animator = transform.GetChild(0).GetComponent<Animator>();
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
    /// � ��ü�� ��ȣ�ۿ������� ������ �Լ�
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
                equipWeapon.gameObject.SetActive(false);
            weaponIndex = select-1;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            animator.SetTrigger(Swap_Hash);
        }
    }
}
