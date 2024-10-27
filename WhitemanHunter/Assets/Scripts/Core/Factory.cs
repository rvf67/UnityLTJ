using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    BulletCasePool bulletCase;
    HandGunBulletPool handBullet;
    SubMachineGunBulletPool subMachineBullet;
    MissilePool missile;
    BossRockPool bossRock;
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

        missile = GetComponentInChildren<MissilePool>();
        if ( missile != null )
            missile?.Initialize();

        bossRock = GetComponentInChildren<BossRockPool>();
        if( bossRock != null )
            bossRock?.Initialize();
    }

    public Bullet GetHandBullet(Vector3? position = null, Vector3? eulerAngle = null)
    {
        Bullet handBulletPrefab = handBullet.GetObject(position, eulerAngle);
        handBulletPrefab.SetDirection((Vector3)eulerAngle);
        return handBulletPrefab;
    }

    public Bullet GetBulletCase(Vector3? position = null, Vector3? eulerAngle = null)
    {
        return bulletCase.GetObject(position, eulerAngle);
    }

    public Bullet GetSubBullet(Vector3? position = null, Vector3? eulerAngle = null)
    {
        Bullet submachineBulletPrefab = subMachineBullet.GetObject(position);
        submachineBulletPrefab.SetDirection((Vector3)eulerAngle);
        return submachineBulletPrefab;
    }

    public Bullet GetYellowMissile(Vector3? position = null, Vector3? eulerAngle =null)
    {
        Bullet missilePrefab= missile.GetObject(position);
        missilePrefab.SetDirection((Vector3)eulerAngle);   
        return missilePrefab;
    }

    public BossRock GetBossRock(Vector3? position = null, Vector3? eulerAngle = null)
    {
        BossRock bossRockPrefab = bossRock.GetObject(position);
        bossRockPrefab.SetDirection((Vector3)eulerAngle);
        return bossRockPrefab;
    }
}
