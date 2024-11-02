using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    /// <summary>
    /// 저장된 최고 점수
    /// </summary>
    TextMeshProUGUI maxScoreText;
    /// <summary>
    /// 시작버튼
    /// </summary>
    Button startButton;


    private void Awake()
    {
        //변수 세팅
        maxScoreText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        startButton = transform.GetChild(3).GetComponent<Button>();

        maxScoreText.text = string.Format("{0:n0}",PlayerPrefs.GetInt("MaxScore"));
        startButton.onClick.AddListener(FindAnyObjectByType<GameManager>().GameStart);
    }
}
