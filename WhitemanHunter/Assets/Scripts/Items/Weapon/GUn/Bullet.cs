using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// �Ѿ��� ������
    /// </summary>
    public int damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")       // ź���� ��� ó��
        {
            Destroy(gameObject,0.3f);
        }   
        else if(collision.gameObject.tag == "Wall"){
            Destroy(gameObject);
        }
    }
}
