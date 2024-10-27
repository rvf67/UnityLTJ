using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossRock : Bullet
{

    /// <summary>
    /// 바위 회전속도
    /// </summary>
    float angularPower = 2f;
    /// <summary>
    /// 크기의 변화를 위한 변수
    /// </summary>
    float scaleValue = 0.1f;
    /// <summary>
    /// 발사여부 확인
    /// </summary>
    bool isShoot=false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    protected override void OnReset()
    {
        base.OnReset();
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        transform.localScale = Vector3.one;
        isShoot = false;
        rb.AddTorque(0, 0, 0);
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            rb.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }

}
