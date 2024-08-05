using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // Start is alled before the first frame update
    GameObject penguin;
    GameObject heart;
    /// <summary>
    /// ü�°��� �ۼ�������
    /// </summary>
    public float decreaseHPPercent = 0.1f;
    void Start()
    {
        this.penguin = GameObject.FindGameObjectWithTag("Player");
        this.heart = GameObject.Find("Heart");

    }
    /// <summary>
    /// ü���� ���Ҹ� UI�� ���� �����ְ� ü���� 0���Ϸ� �������� ���ӿ����� �ǵ��� ��
    /// </summary>
    public void DecreaseHp()
    {
        this.heart.GetComponent<Image>().fillAmount -= decreaseHPPercent/100;
        if (this.heart.GetComponent<Image>().fillAmount <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }


    // Update is called once per frame
}
