using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    BulletCasePool bulletCase;
    HandGunBulletPool handBullet;
    SubMachineGunBulletPool subMachineBullet;

    protected override void OnInitialize()
    {
        handBullet = GetComponentInChildren<HandGunBulletPool>();
        if( handBullet != null )
            handBullet?.Initialize();

        bulletCase = GetComponentInChildren<BulletCasePool>();
        if( bulletCase != null )
            bulletCase?.Initialize();

        subMachineBullet = GetComponentInChildren<SubMachineGunBulletPool>();
        if( subMachineBullet != null )
            subMachineBullet?.Initialize();
    }

    public Bullet GetHandBullet(Vector3? position = null, Vector3? eulerAngle = null)
    {
        return handBullet.GetObject(position, eulerAngle);
    }

    public Bullet GetBulletCase(Vector3? position = null, Vector3? eulerAngle = null)
    {
        return bulletCase.GetObject(position, eulerAngle);
    }

    public Bullet GetSubBullet(Vector3? position = null, Vector3? eulerAngle = null)
    {
        return subMachineBullet.GetObject(position, eulerAngle);
    }
}
