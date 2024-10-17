using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    SharkPool shark;



    protected override void OnInitialize()
    {
        shark = GetComponentInChildren<SharkPool>();
        if (shark != null )
            shark.Initialize();
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

}
