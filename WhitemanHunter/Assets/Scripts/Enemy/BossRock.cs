using System.Collections;
using UnityEngine;


public class BossRock : Bullet
{

    /// <summary>
    /// 기본 바위 회전속도
    /// </summary>
    public float angularBase = 2f;
    /// <summary>
    /// 바위 회전 증가량
    /// </summary>
    public float angularPower = 0.1f;
    
    /// <summary>
    /// 크기의 변화를 위한 변수
    /// </summary>
    public float scaleBase = 0.1f;
    /// <summary>
    /// 크기 증가량
    /// </summary>
    public float scaleValue = 0.01f;
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
        if (GameManager.Instance.Player != null)
        {
            DisableTimer(20.0f);
        }
        transform.localScale = Vector3.one;
        isShoot = false;
        rb.velocity = Vector3.zero; //이것을 하지 않으면 튕겨나감
        rb.angularVelocity = Vector3.zero;
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(3.0f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        float angular=angularBase;
        float scale=scaleBase;    
        while (!isShoot)
        {
            angular += angularPower;
            scale += scaleValue;
            transform.localScale = Vector3.one * scale;
            rb.AddTorque(transform.right * angular, ForceMode.Acceleration);
            yield return null;
        }
    }

}
