using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Killzone : MonoBehaviour
{
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
