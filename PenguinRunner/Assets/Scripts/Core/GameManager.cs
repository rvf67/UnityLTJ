using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float health;

    GameObject heart;
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindAnyObjectByType<Player>();
            }
            return player;
        }
    }


    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        this.heart = GameObject.Find("Heart");
    }
    public void DecreaseHp(float hp)
    {
        this.heart.GetComponent<Image>().fillAmount = hp;
        if (this.heart.GetComponent<Image>().fillAmount <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

 
}
