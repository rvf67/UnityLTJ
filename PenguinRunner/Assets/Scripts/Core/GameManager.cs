using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    GameObject heart;
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    GameOverTime gameOverTime;
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

    public GameOverTime GameOverTime
    {
        get
        {
            if(gameOverTime==null)
            {
                gameOverTime = FindAnyObjectByType<GameOverTime>();
            }
            return gameOverTime;
        }
    }
    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        gameOverTime = FindAnyObjectByType<GameOverTime>();
        this.heart = GameObject.Find("Heart");
    }
    public void DecreaseHp(float hp)
    {
        this.heart.GetComponent<Image>().fillAmount = hp;
        if (this.heart.GetComponent<Image>().fillAmount <= 0)
        {
            gameOverTime.level = 1;
            SceneManager.LoadScene("GameOverScene");
        }
    }

}
