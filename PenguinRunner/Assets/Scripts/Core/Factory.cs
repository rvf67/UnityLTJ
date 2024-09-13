using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    SpikePool spike;
    SharkPool shark;
    MissilePool missile;
    // 풀 초기화
    //bullet = GetComponentInChildren<BulletPool>();
    //    if (bullet != null)
    //        bullet.Initialize();
    protected override void OnInitialize()
    {
        spike = GetComponentInChildren<SpikePool>();
        if (spike != null ) 
            spike.Initialize();

        shark = GetComponentInChildren<SharkPool>();
        if( shark != null )
            shark.Initialize();

        missile = GetComponentInChildren<MissilePool>();
        if(missile != null )
            missile.Initialize();
    }

    public Spike GetSpike(Vector3? position)
    {
        //Vector3.forward * angle
        return spike.GetObject(position,new Vector3(0,0,180.0f));
    }

    public Shark GetShark(Vector3? position)
    {
        Shark sharkModif=shark.GetObject(position);
        sharkModif.SharkFlip();
        return sharkModif;
    }

    public Missile GetMissile(Vector3? position) 
    {
        return missile.GetObject(position); 
    }
}
