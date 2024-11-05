using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    /// <summary>
    /// �޴� ī�޶�
    /// </summary>
    MenuClass menuCam;
    /// <summary>
    /// ���� ī�޶�
    /// </summary>
    CameraFollow gameCam;

    /// <summary>
    /// enemyZones
    /// </summary>
    Transform[] enemyZones;
    /// <summary>
    /// ��ȯ�� �� ����Ʈ
    /// </summary>
    List<int> enemyList;

    /// <summary>
    /// ����
    /// </summary>
    EnemyBoss boss;
    /// <summary>
    /// �÷��̾�
    /// </summary>
    Player player;
    public Player Player => player;
    public EnemyBoss Boss => boss;

    /// <summary>
    /// �޴��г�
    /// </summary>
    MenuPanel menuPanel;
    /// <summary>
    /// �����г�
    /// </summary>
    GamePanel gamePanel;

    GameOverPanel gameOverPanel;
    
    public MenuPanel MenuPanel => menuPanel;
    public GamePanel GamePanel => gamePanel;
    public GameOverPanel GameOverPanel => gameOverPanel;
   


    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
    }

    protected override void OnInitialize()
    {
        StopAllCoroutines();
        player = FindAnyObjectByType<Player>();
        boss = FindAnyObjectByType<EnemyBoss>();
        menuPanel = FindAnyObjectByType<MenuPanel>();
        gamePanel = FindAnyObjectByType<GamePanel>();
        gameOverPanel = FindAnyObjectByType<GameOverPanel>();
        gameCam = FindAnyObjectByType<CameraFollow>();
        menuCam = FindAnyObjectByType<MenuClass>();
    }

    private void Awake()
    {
        
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore",0);
    }

    private void Start()
    {
        //���ӽ����� ����
        menuPanel.ScoreUpdate();
        player.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        boss.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(false);
        enemyList = new List<int>();
        enemyZones = GameObject.Find("Spawners").transform.GetComponentsInChildren<Transform>();
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// �������� ���� �Լ�
    /// </summary>
    /// <param name="stageZone">��û ��������</param>
    public void StageStart(GameObject stageZone)
    {
        stageZone.SetActive(false);
        gamePanel.isBattle = true;

        foreach ( Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }
        player.gameObject.layer = player.GetComponent<PlayerInteraction>().PlayerLayer;
        StartCoroutine(InBattle(stageZone));
    }

    /// <summary>
    /// �������� ���� �Լ�
    /// </summary>
    /// <param name="stageZone">��û ��������</param>
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
        player.gameObject.layer = player.GetComponent<PlayerInteraction>().UndieLayer;
    }
    
    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public void GameStart()
    {
        menuCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);

        menuPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gamePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.ShowScore();
    }

    /// <summary>
    /// ��Ʋ���� �ڷ�ƾ
    /// </summary>
    /// <param name="stageZone">��û ��������</param>
    /// <returns></returns>
    IEnumerator InBattle(GameObject stageZone)
    {
        if (gamePanel.Stage % 5 == 0)
        {
            gamePanel.EnemyCntD++;
            boss.transform.position = enemyZones[4].position;
            boss.gameObject.SetActive(true);
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
                int ranZone = Random.Range(1, 5);
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
