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
    /// �÷��̾�
    /// </summary>
    Player player;
    /// <summary>
    /// ���� ǥ�ÿ� UI
    /// </summary>
    ScoreText scoreTextUI;
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

    public ScoreText ScoreText
    {
        get
        {
            if (scoreTextUI == null)
            {
                scoreTextUI = FindAnyObjectByType<ScoreText>();
            }
            return scoreTextUI;
        }
    }

    /// <summary>
    /// ScoreText�� score�� Ȯ���ϴ� ������Ƽ
    /// </summary>
    public int Score => ScoreText.Score;    // get�� �ִ� ������Ƽ

    private void Awake()
    {
        this.heart = GameObject.Find("Heart");
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();

        scoreTextUI = FindAnyObjectByType<ScoreText>();
        scoreTextUI?.OnInitialize();
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

    /// <summary>
    /// ���� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="score">�߰��Ǵ� ����</param>
    public void AddScore(int score)
    {
        ScoreText?.AddScore(score);
    }
}
