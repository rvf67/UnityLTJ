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
    BossMissilePool bossMissile;

    EnemyAPool enemyA;
    EnemyBPool enemyB;
    EnemyCPool enemyC;
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

        bossMissile = GetComponentInChildren<BossMissilePool>();
        if( bossMissile != null ) 
            bossMissile?.Initialize();

        enemyA = GetComponentInChildren<EnemyAPool>();
        if( enemyA != null )
            enemyA?.Initialize();

        enemyB = GetComponentInChildren<EnemyBPool>();
        if (enemyB != null)
            enemyB?.Initialize();

        enemyC = GetComponentInChildren<EnemyCPool>();
        if ( enemyC != null )
            enemyC?.Initialize();
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

    public BossMissile GetBossMissile(Vector3? position =null, Vector3? eulerAngle = null)
    {
        BossMissile bossMissilePrefab = bossMissile.GetObject(position);
        bossMissilePrefab.SetDirection((Vector3)eulerAngle);
        return bossMissilePrefab;
    }
    public GreenEnemy GetGreenEnemy(Vector3? position = null)
    {
        return enemyA.GetObject(position);
    }
    public PurpleEnemy GetPurpleEnemy(Vector3? position = null)
    {
        return enemyB.GetObject(position);
    }
    public YellowEnemy GetYellowEnemy(Vector3? position = null)
    {
        return enemyC.GetObject(position);
    }
}
