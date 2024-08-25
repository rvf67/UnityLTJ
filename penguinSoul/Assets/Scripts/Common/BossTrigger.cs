using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject panelBossHP;

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
            panelBossHP.SetActive(true);
            collision.gameObject.transform.GetChild(3).SetParent(null);
            this.gameObject.SetActive(false);
        }
    }
}
