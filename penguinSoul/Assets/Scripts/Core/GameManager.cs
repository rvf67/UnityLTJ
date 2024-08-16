using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int stageIndex;
    public float health;
    GameObject heart;
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    private void Awake()
    {
        this.heart = GameObject.Find("Heart");
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
    }
    public void DecreaseHp(float hp)
    {
        this.heart.GetComponent<Image>().fillAmount = hp;
        if (this.heart.GetComponent<Image>().fillAmount <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
    public void NextStage()
    {
        stageIndex++;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
