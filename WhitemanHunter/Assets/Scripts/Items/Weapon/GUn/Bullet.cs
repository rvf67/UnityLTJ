using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    /// <summary>
    /// 총알의 데미지
    /// </summary>
    public int damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")       // 탄피일 경우 처리
        {
            Destroy(gameObject,0.3f);
        }   
        else if(collision.gameObject.tag == "Wall"){
            Destroy(gameObject);
        }
    }
}
