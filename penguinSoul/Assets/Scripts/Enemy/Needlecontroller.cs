using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Needlecontroller : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        this.player = GameObject.Find("penguin");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D player)
    {
        //GameObject dierctor = GameObject.Find("GameDirector");
        //dierctor.GetComponent<GameManager>().DecreaseHp();
        gameObject.SetActive(false);
    }
}
