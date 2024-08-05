using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("penguin");

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPos = player.transform.position;
        transform.position = new Vector3(PlayerPos.x + 7,
            transform.position.y, transform.position.z);

    }
}



