using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SharkController : RecycleObject
{
    /// <summary>
    /// ����� �ӵ�
    /// </summary>
    float moveSpeed=-0.04f;
    
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        transform.Translate(moveSpeed,0,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        spriteRenderer.flipX = moveSpeed < 0;
        moveSpeed=-moveSpeed; //������ �ٲٱ� ����
    }

    
}
