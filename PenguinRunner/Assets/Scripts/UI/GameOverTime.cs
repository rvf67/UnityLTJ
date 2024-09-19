using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTime : MonoBehaviour
{
    /// <summary>
    /// �����ð� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI countText;

    /// <summary>
    /// �����ð�
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
