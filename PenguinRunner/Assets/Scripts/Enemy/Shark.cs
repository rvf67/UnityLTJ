using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Shark : EnemyBase
{
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void SharkFlip()
    {
        if(GameManager.Instance.Player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
            moveSpeed = moveSpeed * -1;
        }
        else
        {
            spriteRenderer.flipX=false;
            moveSpeed = Mathf.Abs(moveSpeed);
        }
    }
}
