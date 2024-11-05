using System.Collections;
using UnityEngine;


public class BossRock : Bullet
{

    /// <summary>
    /// �⺻ ���� ȸ���ӵ�
    /// </summary>
    public float angularBase = 2f;
    /// <summary>
    /// ���� ȸ�� ������
    /// </summary>
    public float angularPower = 0.1f;
    
    /// <summary>
    /// ũ���� ��ȭ�� ���� ����
    /// </summary>
    public float scaleBase = 0.1f;
    /// <summary>
    /// ũ�� ������
    /// </summary>
    public float scaleValue = 0.01f;
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
        if (GameManager.Instance.Player != null)
        {
            DisableTimer(20.0f);
        }
        transform.localScale = Vector3.one;
        isShoot = false;
        rb.velocity = Vector3.zero; //�̰��� ���� ������ ƨ�ܳ���
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
