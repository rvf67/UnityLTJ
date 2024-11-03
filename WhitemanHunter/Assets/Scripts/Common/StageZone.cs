using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageZone : MonoBehaviour
{
    GameManager gameManager;


    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            gameManager.StageStart(this.gameObject);
    }
}
