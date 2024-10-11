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
    /// �÷��̾��� ������ ������Ʈ
    /// </summary>
    PlayerMovement playerMovement;

    /// <summary>
    /// �÷��̾ ���� �����
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
}
