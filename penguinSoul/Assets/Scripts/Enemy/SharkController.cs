using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SharkController : RecycleObject
{


    // Update is called once per frame
    void Update()
    {
        transform.Translate(-0.04f, 0, 0);
        if (transform.position.x < -11.0f) 
        {
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D player)
    {
        //GameObject dierctor = GameObject.Find("GameDirector");
        //dierctor.GetComponent<GameManager>().DecreaseHp();
        Debug.Log("attack");
    }
}
