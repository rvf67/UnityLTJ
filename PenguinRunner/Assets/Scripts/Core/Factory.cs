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
    /// ��� ����
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
    /// ������ �̻��� �ϳ��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public BossMissile GetBossMissile(Vector3? position)
    {
        return bossMissile.GetObject(position);
    }

    /// <summary>
    /// ���� �����ϰ� ������ direction�������� ����
    /// </summary>
    /// <param name="position">������ġ</param>
    /// <param name="direction">���� ����</param>
    /// <returns></returns>
    public BossBall GetBossBall(Vector3? position, Vector2 direction)
    {
        BossBall bb = bossBall.GetObject(position);
        bb.SetDirection(direction);
        return bb;
    }

    /// <summary>
    /// ������ ����� �������ִ� �Լ�
    /// </summary>
    /// <param name="position">������ġ</param>
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
