using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    /// <summary>
    /// 바위 리지드바디
    /// </summary>
    Rigidbody rb;
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
