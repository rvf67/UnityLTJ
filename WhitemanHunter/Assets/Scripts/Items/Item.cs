using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    public float rotateSpeed = 10.0f;
    /// <summary>
    /// ��ü�� ���ٴϴ� �ӵ�(���Ʒ�)
    /// </summary>
    public float floatSpeed = 0.5f;
    /// <summary>
    /// ��ü�� ������ �ð�
    /// </summary>
    public float floatTime = 1.0f;
    /// <summary>
    /// ���ζ������
    /// </summary>
    bool isUp=true;
    /// <summary>
    /// ������ ����
    /// </summary>
    Vector3 direction;
    /// <summary>
    /// ������ Ÿ��
    /// </summary>
    public enum ItemType
    {
        Ammo =0,
        Coin,
        Grenade,
        Heart,
        Weapon
    };
    /// <summary>
    /// ���� Ÿ���� ������ ����
    /// </summary>
    public ItemType itemType;
    /// <summary>
    /// �������� ������ ��
    /// </summary>
    public int value;
    private void Start()
    {
        StartCoroutine(UpDown());
    }
    private void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);            //������ ȸ��
        transform.GetChild(0).Translate(direction*floatSpeed*Time.deltaTime,Space.World);
    }

    /// <summary>
    /// ��ü�� ���Ʒ��� �����̰��ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator UpDown()
    {
        while (true) 
        {
            if (isUp)
            {
                direction = Vector3.up;
                isUp=false;
            }
            else
            {
                direction = Vector3.down;
                isUp=true;
            }
            yield return new WaitForSeconds(floatTime);
        }
    }
}
