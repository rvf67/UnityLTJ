using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;
    public Vector3 offset;
    // Update is called once per frame
    private void Start()
    {
        target = GameManager.Instance.Player.transform;
    }
    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
