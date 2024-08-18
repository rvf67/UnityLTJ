using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SharkController : EnemyBase
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        transform.Translate(moveSpeed,0,0);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Player")||collision.gameObject.CompareTag("Map"))
        {
            moveSpeed=-moveSpeed; //방향을 바꾸기 위함
            spriteRenderer.flipX = moveSpeed > 0;
        }
    }

    protected override void OnDie()
    {
        GameManager.Instance.AddScore(point);
    }
}
