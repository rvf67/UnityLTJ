using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Factory.Instance.GetDangerLine(transform.GetChild(0).transform.position);
        }
    }
}
