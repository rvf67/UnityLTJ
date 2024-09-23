using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverTime : MonoBehaviour
{
    /// <summary>
    /// 남은시간 텍스트
    /// </summary>
    TextMeshProUGUI countText;
    /// <summary>
    /// 흐른시간
    /// </summary>
    float flowTime;
    /// <summary>
    /// 남은시간
    /// </summary>
    public float countDown=60.0f;
    /// <summary>
    /// 최대시간
    /// </summary>
    public float maxTime;
    /// <summary>
    /// 올라가는 난이도에 사용하는 난이도
    /// </summary>
    public int level=1;
    /// <summary>
    /// 최대 난이도
    /// </summary>
    public int maxLevel=3;
    private void Awake()
    {
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        maxTime=countDown;
    }

    private void Update()
    {
        countDown -= Time.deltaTime;
        flowTime += Time.deltaTime;
        if (flowTime > maxTime / maxLevel && level < maxLevel)
        {
            level++;
            flowTime = 0;
        }
        countText.text = $"{countDown.ToString("F1")}";

        if(countDown<0)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
