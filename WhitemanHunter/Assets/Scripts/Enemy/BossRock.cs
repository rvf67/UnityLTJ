using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    /// <summary>
    /// ���� ������ٵ�
    /// </summary>
    Rigidbody rb;
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
    bool isShoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
    }

    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            yield return null;
        }
    }

}
