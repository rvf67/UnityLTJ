using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public int health;

    public void NextStage()
    {
        stageIndex++;


    }

    private void Update()
    {
        
    }
}
