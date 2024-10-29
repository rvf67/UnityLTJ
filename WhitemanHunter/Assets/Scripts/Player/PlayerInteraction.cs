using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// �����̿� �ִ� ��ȣ�ۿ��� ������Ʈ
    /// </summary>
    GameObject nearObject;
    /// <summary>
    /// ���� ���̾�
    /// </summary>
    int undieLayer;
    /// <summary>
    /// �÷��̾� ���̾�
    /// </summary>
    int playerLayer;
    /// <summary>
    /// �����ϰ� �ִ� ����
    /// </summary>
    public Weapon equipWeapon;

    /// <summary>
    /// ������ �ִ� ����ź��
    /// </summary>
    int ammo;
    /// <summary>
    /// ���Ÿ� ���Ⱑ ���� �� �ִ� �ִ� ź��
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
    /// ����ü��
    /// </summary>
    float health;
    /// <summary>
    /// ������ �ִ� �ִ�ü��
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
    /// ������ �ִ� ��
    /// </summary>
    int coin;
    /// <summary>
    /// ������ �ִ� �ִ� ��
    /// </summary>
    public int coinMax;

    /// <summary>
    /// ���� �ִϸ����Ϳ� �ؽ�
    /// </summary>
    readonly int Swap_Hash = Animator.StringToHash("Swap");
    /// <summary>
    /// �÷��̾ ���� �����
    /// </summary>
    public GameObject[] weapons;
    /// <summary>
    /// �����ϰ� �ִ����� ���� �迭
    /// </summary>
    public bool[] hasWeapons;
    /// <summary>
    /// ���������� üũ�ϴ� ����
    /// </summary>
    public bool isSwap;
    /// <summary>
    /// �����ð�
    /// </summary>
    public float undieTime = 1.0f;
    /// <summary>
    /// �¾Ҵ��� ����
    /// </summary>
    public bool isDamage;
    /// <summary>
    /// �÷��̾��� ������ ������Ʈ
    /// </summary>
    PlayerMovement playerMovement;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator animator;
    /// <summary>
    /// �÷��̾� �޽õ�
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
                    Debug.Log("�̱��� ���� ���� ����");
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

    /// <summary>
    /// ���⽺�� �Լ�
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
                if(equipWeapon.gameObject==weapons[weaponIndex])        //������ ���Ⱑ ������ ����� ������
                {
                    equipWeapon.gameObject.SetActive(false);
                    equipWeapon = null;                                 //���� ����
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
    /// ���� ���� ���� �Լ�
    /// </summary>
    public void SwapOut()
    {
        isSwap = false;
    }

    public void Die()
    {
        Debug.Log("�״�.");
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
                //30.0f�� �����̴� �ӵ� ������
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
