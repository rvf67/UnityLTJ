using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    /// <summary>
    /// ȸ���ӵ�
    /// </summary>
    public float rotateSpeed=30.0f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
}
