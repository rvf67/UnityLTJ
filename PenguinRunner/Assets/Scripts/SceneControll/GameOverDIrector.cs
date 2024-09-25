using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverDIrector : MonoBehaviour
{
    public void OnInitialize()
    {
        Player player = GameManager.Instance.Player;
        GameOverTime gameOverTime = GameManager.Instance.GameOverTime;
        GameObject heart = GameObject.Find("Heart");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
