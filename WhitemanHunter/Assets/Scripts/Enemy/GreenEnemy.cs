using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        bodyMaterial = GetComponentInChildren<MeshRenderer>().material;
    }
}
