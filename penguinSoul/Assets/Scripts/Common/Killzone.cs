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
        // GetComponent�� ���� �� <>������ Ŭ������ �� Ŭ������ ��ӹ��� Ŭ������ ������ return�� null
        if (collision.GetComponent<RecycleObject>() != null)
        {
            // ������Ŭ ������Ʈ
            collision.gameObject.SetActive(false);
        }
        else
        {
            // �Ϲ� ������Ʈ
        }
    }
}
