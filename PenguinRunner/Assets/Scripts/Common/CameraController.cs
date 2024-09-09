using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rayMaxDistance;
    Camera cam;
    Vector2 mousePosition;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, rayMaxDistance);
        if (hit)
        {
            Debug.Log("hit");
        }
    }
}
