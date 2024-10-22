using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 회전속도
    /// </summary>
    public float rotateSpeed = 10.0f;
    /// <summary>
    /// 물체가 떠다니는 속도(위아래)
    /// </summary>
    public float floatSpeed = 0.5f;
    /// <summary>
    /// 물체가 움직일 시간
    /// </summary>
    public float floatTime = 1.0f;
    /// <summary>
    /// 위로뜰것인지
    /// </summary>
    bool isUp=true;
    /// <summary>
    /// 움직일 방향
    /// </summary>
    Vector3 direction;
    /// <summary>
    /// 아이템 타입
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
    /// 실제 타입을 저장할 변수
    /// </summary>
    public ItemType itemType;
    /// <summary>
    /// 아이템이 가지는 양
    /// </summary>
    public int value;
    private void Start()
    {
        StartCoroutine(UpDown());
    }
    private void Update()
    {
        transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime);            //옆으로 회전
        transform.GetChild(0).Translate(direction*floatSpeed*Time.deltaTime,Space.World);
    }

    /// <summary>
    /// 물체를 위아래로 움직이게하는 코루틴
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
