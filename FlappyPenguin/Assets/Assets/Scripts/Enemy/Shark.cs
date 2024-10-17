using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Shark : RecycleObject
{
    // Start is called before the first frame update
    public GameObject sharkPrefab;
    GameObject penguin;
    float span = 3.0f;
    float delta = 0;
    void Start()
    {
        this.penguin = GameObject.Find("penguin");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 p1= this.penguin.transform.position;
        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.span = Random.Range(3.0f, 10.0f);
            this.delta = 0;
            int px = Random.Range(-4, 4);
            transform.position = new Vector3(p1.x + 35.0f, px, 0);
        }
    }
}
