using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    GameManager gameManager;

    Camera cam;
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        cam=Camera.main;
    }
    private void Update()
    {
        float dirx = cam.transform.position.x;
        transform.position=new Vector2(dirx, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // GetComponent를 했을 때 <>사이의 클래스나 그 클래스를 상속받은 클래스가 없으면 return은 null
        if (collision.GetComponent<RecycleObject>() != null)
        {
            // 리사이클 오브젝트
            collision.gameObject.SetActive(false);
        }
        else
        {
            // 일반 오브젝트
        }
    }
}
