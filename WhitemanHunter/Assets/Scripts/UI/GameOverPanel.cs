using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    TextMeshProUGUI curScore;
    TextMeshProUGUI bestText;
    Button restartButton;

    private void Awake()
    {
        Transform child = transform.GetChild(3);
        curScore = child.GetComponent<TextMeshProUGUI>();
        child= transform.GetChild(1);
        bestText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        restartButton = child.GetComponent<Button>();
    }
    private void Start()
    {
        restartButton.onClick.AddListener(()=>SceneManager.LoadScene(0));
    }
    /// <summary>
    /// 점수를 보여주는 함수
    /// </summary>
    public void ShowScore()
    {
        PlayerInteraction playerInteraction =GameManager.Instance.Player.GetComponent<PlayerInteraction>();
        curScore.text = playerInteraction.Score.ToString();
        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (playerInteraction.Score > maxScore)
        {
            bestText.enabled = true;
            PlayerPrefs.SetInt("MaxScore", playerInteraction.Score);
        }
    }
}
