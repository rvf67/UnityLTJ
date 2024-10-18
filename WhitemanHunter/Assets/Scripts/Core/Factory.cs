using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    BulletPool bullet;

    protected override void OnInitialize()
    {
        bullet = GetComponentInChildren<BulletPool>();
        bullet?.Initialize();
    }

    public Bullet GetBullet(Vector3? position = null, Vector3? eulerAngle = null)
    {
        return bullet.GetObject(position, eulerAngle);
    }
}
