using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTime : MonoBehaviour
{
    /// <summary>
    /// 남은시간 텍스트
    /// </summary>
    TextMeshProUGUI countText;

    /// <summary>
    /// 남은시간
    /// </summary>
    public float countDown=60.0f;

    private void Awake()
    {
        countText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        countDown -= Time.deltaTime;
        countText.text = $"{countDown}";
    }
}
