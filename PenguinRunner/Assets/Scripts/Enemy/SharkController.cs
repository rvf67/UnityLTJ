using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SharkController : EnemyBase
{
    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        OnMoveUpdate(Time.deltaTime);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")||collision.gameObject.CompareTag("Map"))
        {
            point = 0;
            transform.gameObject.SetActive(false);
        }
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnDie()
    {
        GameManager.Instance.AddScore(point);
    }
}
