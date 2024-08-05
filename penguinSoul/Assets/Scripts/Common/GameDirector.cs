using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // Start is alled before the first frame update
    GameObject penguin;
    GameObject heart;
    /// <summary>
    /// 체력감소 퍼센테이지
    /// </summary>
    public float decreaseHPPercent = 0.1f;
    void Start()
    {
        this.penguin = GameObject.FindGameObjectWithTag("Player");
        this.heart = GameObject.Find("Heart");

    }
    /// <summary>
    /// 체력의 감소를 UI를 통해 보여주고 체력이 0이하로 내려가면 게임오버가 되도록 함
    /// </summary>
    public void DecreaseHp()
    {
        this.heart.GetComponent<Image>().fillAmount -= decreaseHPPercent/100;
        if (this.heart.GetComponent<Image>().fillAmount <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }


    // Update is called once per frame
}
