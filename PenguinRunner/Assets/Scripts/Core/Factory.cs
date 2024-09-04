using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    SpikePool spike;
    // Ǯ �ʱ�ȭ
    //bullet = GetComponentInChildren<BulletPool>();
    //    if (bullet != null)
    //        bullet.Initialize();
    protected override void OnInitialize()
    {
        spike = GetComponentInChildren<SpikePool>();
        if (spike != null ) 
            spike.Initialize();
    }

    public Spike GetSpike(Vector3? position)
    {
        //Vector3.forward * angle
        return spike.GetObject(position);
    }
}
