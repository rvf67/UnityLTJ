using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject menuCam;
    public GameObject gameCam;

    EnemyBoss boss;
    Player player;
    public Player Player => player;
    public EnemyBoss Boss => boss;

    MenuPanel menuPanel;
    GamePanel gamePanel;
    
    public MenuPanel MenuPanel => menuPanel;
    public GamePanel GamePanel => gamePanel;
   

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        boss = FindAnyObjectByType<EnemyBoss>();
        menuPanel = FindAnyObjectByType<MenuPanel>();
        gamePanel = FindAnyObjectByType<GamePanel>();
        //시작전 세팅
        gameCam.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        boss.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        menuCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);

        menuPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);

        player.gameObject.SetActive(true);
    }
}
