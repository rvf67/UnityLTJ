using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Melee,
        Range
    }
    public WeaponType type;
    public float damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

}
