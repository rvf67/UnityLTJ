using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    /// <summary>
    /// ����� �ְ� ����
    /// </summary>
    TextMeshProUGUI maxScoreText;
    /// <summary>
    /// ���۹�ư
    /// </summary>
    Button startButton;


    private void Awake()
    {
        //���� ����
        maxScoreText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        startButton = transform.GetChild(3).GetComponent<Button>();

        maxScoreText.text = string.Format("{0:n0}",PlayerPrefs.GetInt("MaxScore"));
        startButton.onClick.AddListener(FindAnyObjectByType<GameManager>().GameStart);
    }
}
