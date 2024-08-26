using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            Transform cam = collision.gameObject.transform.GetChild(3);
            cam.SetParent(null);
            cam.transform.position = new Vector3(cam.position.x,-5.8f,cam.position.z);
            this.gameObject.SetActive(false);
        }
    }
}
