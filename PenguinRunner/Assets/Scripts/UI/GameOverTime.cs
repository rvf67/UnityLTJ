using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverTime : MonoBehaviour
{
    /// <summary>
    /// �����ð� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI countText;
    /// <summary>
    /// �帥�ð�
    /// </summary>
    float flowTime;
    /// <summary>
    /// �����ð�
    /// </summary>
    public float countDown=60.0f;
    /// <summary>
    /// �ִ�ð�
    /// </summary>
    public float maxTime;
    /// <summary>
    /// �ö󰡴� ���̵��� ����ϴ� ���̵�
    /// </summary>
    public int level=1;
    /// <summary>
    /// �ִ� ���̵�
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
