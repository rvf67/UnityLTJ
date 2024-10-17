using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // Start is alled before the first frame update
    GameObject penguin;
    GameObject flag;
    GameObject distance;
    GameObject heart;
    void Start()
    {
        this.penguin = GameObject.Find("penguin");
        this.flag = GameObject.Find("flag");
        this.distance = GameObject.Find("Distance");
        this.heart = GameObject.Find("Heart");

    }
    public void DecreaseHp()
    {
        this.heart.GetComponent<Image>().fillAmount -= 0.1f;
        penguin.GetComponent<PenguinController>().Damage();
        if (this.heart.GetComponent<Image>().fillAmount <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }


    // Update is called once per frame
    void Update()
    {
        float length = this.flag.transform.position.x - this.penguin.transform.position.x;
        this.distance.GetComponent<Text>().text = "목표 지점까지:" + length.ToString("F2") + "m";
        if (length < 0)
        {
            this.distance.GetComponent<Text>().text = "골";
        }
    }
}
