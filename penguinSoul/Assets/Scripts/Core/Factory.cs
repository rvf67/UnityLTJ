using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    SharkPool shark;
    PlayerBulletPool playerBullet;
    BossMissilePool bossMissile;
    BossBallPool bossBall;
    DangerLinePool dangerLine;
    protected override void OnInitialize()
    {
        shark = GetComponentInChildren<SharkPool>();
        if (shark != null )
            shark.Initialize();

        playerBullet = GetComponentInChildren<PlayerBulletPool>();
        if( playerBullet != null )
            playerBullet.Initialize();

        bossMissile = GetComponentInChildren<BossMissilePool>();
        if(bossMissile != null ) 
            bossMissile.Initialize();

        bossBall = GetComponentInChildren<BossBallPool>();
        if(bossBall != null )
            bossBall.Initialize();

        dangerLine = GetComponentInChildren<DangerLinePool>();
        if(dangerLine != null )
            dangerLine.Initialize();

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

    /// <summary>
    /// 보스용 미사일 하나를 리턴하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BossMissile GetBossMissile(Vector3? position)
    {
        return bossMissile.GetObject(position);
    }

    /// <summary>
    /// 공을 생성하고 방향을 direction방향으로 지정
    /// </summary>
    /// <param name="position">스폰위치</param>
    /// <param name="direction">스폰 방향</param>
    /// <returns></returns>
    public BossBall GetBossBall(Vector3? position, Vector2 direction)
    {
        BossBall bb = bossBall.GetObject(position);
        bb.SetDirection(direction);
        return bb;
    }

    /// <summary>
    /// 돌진전 경고선을 스폰해주는 함수
    /// </summary>
    /// <param name="position">스폰위치</param>
    /// <returns></returns>
    public DangerLine GetDangerLine(Vector3? position)
    {
        DangerLine danger = dangerLine.GetObject(position);
        if (position != null)
        {
            danger.endPosition=GameManager.Instance.Player.transform.position;
            danger.SetDestination(danger.endPosition);
        }
        return danger;
    }
}
