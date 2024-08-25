using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    GameObject boss;
    private void Awake()
    {
        boss = GameObject.Find("Boss").transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boss.SetActive(true);
            collision.gameObject.transform.GetChild(3).SetParent(null);
            this.gameObject.SetActive(false);
        }
    }
}
