using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    protected const float MaxX = 8.0f;
    protected const float MinX = -8.0f;
    private void OnDrawGizmos()
    {
        // 출발지점 그리기
        Gizmos.color = Color.green;
        Vector3 p0 = transform.position + Vector3.right * MaxX;
        Vector3 p1 = transform.position + Vector3.right * MinX;
        Gizmos.DrawLine(p0, p1);
    }
}
