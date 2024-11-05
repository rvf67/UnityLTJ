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
        
        startButton.onClick.AddListener(FindAnyObjectByType<GameManager>().GameStart);
        maxScoreText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

    }

    /// <summary>
    /// 최고점수를 업데이트 해주는 함수
    /// </summary>
    public void ScoreUpdate()
    {
        maxScoreText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }
}
