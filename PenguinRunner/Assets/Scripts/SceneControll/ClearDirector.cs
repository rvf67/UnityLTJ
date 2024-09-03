using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearDirector : MonoBehaviour
{
    public void OnInitialize()
    {
        Player player = GameManager.Instance.Player;
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