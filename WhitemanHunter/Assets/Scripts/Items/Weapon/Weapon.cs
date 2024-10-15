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
    public void Use()
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine(ActivateMelee());
            StartCoroutine(ActivateMelee());
        }
    }
    public WeaponType type;
    public float damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    IEnumerator ActivateMelee()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;    
    }
}
