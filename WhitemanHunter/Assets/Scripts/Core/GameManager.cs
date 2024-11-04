using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    /// <summary>
    /// 메뉴 카메라
    /// </summary>
    public GameObject menuCam;
    /// <summary>
    /// 게임 카메라
    /// </summary>
    public GameObject gameCam;

    /// <summary>
    /// enemyZones
    /// </summary>
    Transform[] enemyZones;
    /// <summary>
    /// 소환할 적 리스트
    /// </summary>
    List<int> enemyList;

    /// <summary>
    /// 보스
    /// </summary>
    EnemyBoss boss;
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    public Player Player => player;
    public EnemyBoss Boss => boss;

    /// <summary>
    /// 메뉴패널
    /// </summary>
    MenuPanel menuPanel;
    /// <summary>
    /// 게임패널
    /// </summary>
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
        enemyList = new List<int>();
        enemyZones = GameObject.Find("Spawners").transform.GetComponentsInChildren<Transform>();
        foreach( Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 스테이지 시작 함수
    /// </summary>
    /// <param name="stageZone">요청 스테이지</param>
    public void StageStart(GameObject stageZone)
    {
        stageZone.SetActive(false);
        gamePanel.isBattle = true;

        foreach ( Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }

        StartCoroutine(InBattle(stageZone));
    }

    /// <summary>
    /// 스테이지 종료 함수
    /// </summary>
    /// <param name="stageZone">요청 스테이지</param>
    public void StageEnd(GameObject stageZone)
    {
        player.transform.GetComponent<PlayerMovement>().Teleport(Vector3.zero);
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }
        stageZone.SetActive(true);
        gamePanel.isBattle=false;
        gamePanel.NextStage();
    }
    
    /// <summary>
    /// 게임 시작 함수
    /// </summary>
    public void GameStart()
    {
        menuCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);

        menuPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);

        player.gameObject.SetActive(true);
    }

    /// <summary>
    /// 배틀시작 코루틴
    /// </summary>
    /// <param name="stageZone">요청 스테이지</param>
    /// <returns></returns>
    IEnumerator InBattle(GameObject stageZone)
    {
        if (gamePanel.Stage % 5 == 0)
        {
            gamePanel.EnemyCntD++;
        }
        else
        {
            for (int index = 0; index < gamePanel.Stage; index++)
            {
                int enemyRandom = Random.Range(0, 3);
                switch (enemyRandom)
                {
                    case 0:
                        gamePanel.EnemyCntA++;
                        break;
                    case 1:
                        gamePanel.EnemyCntB++;
                        break;
                    case 2:
                        gamePanel.EnemyCntC++;
                        break;
                }
                enemyList.Add(enemyRandom);
            }

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                switch (enemyList[0])
                {
                    case 0:
                        Factory.Instance.GetGreenEnemy(enemyZones[ranZone].position);
                        break;
                    case 1:
                        Factory.Instance.GetPurpleEnemy(enemyZones[ranZone].position);
                        break;
                    case 2:
                        Factory.Instance.GetYellowEnemy(enemyZones[ranZone].position);
                        break;
                }

                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(5f);

            }
        }
        while(gamePanel.EnemyCntA +gamePanel.EnemyCntB+gamePanel.EnemyCntC+gamePanel.EnemyCntD > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        StageEnd(stageZone);
    }
}
