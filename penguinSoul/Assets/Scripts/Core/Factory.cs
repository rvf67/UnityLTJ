using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    SharkPool shark;
    PlayerBulletPool playerBullet;


    protected override void OnInitialize()
    {
        shark = GetComponentInChildren<SharkPool>();
        if (shark != null )
            shark.Initialize();

        playerBullet = GetComponentInChildren<PlayerBulletPool>();
        if( playerBullet != null )
            playerBullet.Initialize();
    }
    /// <summary>
    /// 상어 생성
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public SharkController GetShark(Vector3? position)
    {
        return shark.GetObject(position);
    }
    public PlayerBullet GetBullet(Vector3? position,bool isFlip, float angle = 0.0f)
    {
        PlayerBullet bullet = playerBullet.GetObject(position, new Vector3(0, 0, angle));
        if (isFlip)
        {
            bullet.moveSpeed = bullet.moveSpeed * -1;
        }
        else
        {
            bullet.moveSpeed = Mathf.Abs(bullet.moveSpeed);
        }
        return bullet;
    }
}
