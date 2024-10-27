using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossRock : Bullet
{

    /// <summary>
    /// ���� ȸ���ӵ�
    /// </summary>
    float angularPower = 2f;
    /// <summary>
    /// ũ���� ��ȭ�� ���� ����
    /// </summary>
    float scaleValue = 0.1f;
    /// <summary>
    /// �߻翩�� Ȯ��
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
