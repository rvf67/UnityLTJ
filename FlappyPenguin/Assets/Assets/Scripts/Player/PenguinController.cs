using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PenguinController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject flag;
    float speed = 0;
    float speed1 = 0;
    public float maxSpeedVertical = 0.05f;
    public float maxSpeedHorizontal = 0.05f;
    Rigidbody2D rigid2D;
    Animator animator;
    Vector2 startPos;
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.flag = GameObject.Find("flag");
    }

    void Update()
    {
        Vector2 p1 = this.flag.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;
            //좌우의 길이
            float swipeLength = (endPos.x - this.startPos.x);
            //상하의 길이
            float swipeLength1 = (endPos.y - this.startPos.y);
            this.speed = swipeLength / 500.0f;
            //상하의 속도
            this.speed1 = swipeLength1*2.0f;
            if (this.speed1 > maxSpeedHorizontal) { this.speed1 = maxSpeedHorizontal; }
            else if (this.speed1 < 0) { this.speed1 = 0; } //아래로의 이동은 막음
            this.rigid2D.AddForce(transform.up * this.speed1);
            //가로의 속도
            if (this.speed > maxSpeedVertical){this.speed = maxSpeedVertical;}
            else if(this.speed <-maxSpeedVertical){this.speed = -maxSpeedVertical;}
            if(endPos.x < this.startPos.x)
            {
                transform.localScale= new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        transform.Translate(this.speed, 0, 0);

        this.speed *= 0.98f;
        this.speed1 *= 0.98f;
        //애니메이션 속도
        this.animator.speed = Mathf.Abs((speed + speed1) / 100.0f);

        if (transform.position.y < -6.0f)
        {
            SceneManager.LoadScene("GameOverScene");
        }

        if (transform.position.x > p1.x)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
    public void Damage()
    {
        this.animator.SetTrigger("DamageTrigger");
    }
}
