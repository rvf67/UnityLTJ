using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    /// <summary>
    /// 현재 스테이지
    /// </summary>
    int stage;
    public int Stage
    {
        get => stage;
    }
    /// <summary>
    /// 게임 플레이시간
    /// </summary>
    float playTime;
    /// <summary>
    /// 배틀 중인지 
    /// </summary>
    public bool isBattle;
    /// <summary>
    /// 적별 카운트들
    /// </summary>
    int enemyCntA;
    int enemyCntB;
    int enemyCntC;
    public int EnemyCntA
    {
        get => enemyCntA;
        set
        {
            if (enemyCntA != value)
            {
                enemyCntA = value;
            }
        }
            
    }
    public int EnemyCntB
    {
        get => enemyCntB;
        set
        {
            if (enemyCntB != value)
            {
                enemyCntB = value;
            }
        }

    }

    public int EnemyCntC
    {
        get => enemyCntC;
        set
        {
            if (enemyCntC != value)
            {
                enemyCntC = value;
            }
        }

    }
    
    /// <summary>
    /// 플레이어 상호작용
    /// </summary>
    PlayerInteraction playerInteraction;
    /// <summary>
    /// 보스
    /// </summary>
    EnemyBoss enemyBoss;
    /// <summary>
    /// 점수 텍스트
    /// </summary>
    TextMeshProUGUI scoreText;
    /// <summary>
    /// 스테이지 텍스트
    /// </summary>
    TextMeshProUGUI stageText;
    /// <summary>
    /// 풀레이타임 텍스트
    /// </summary>
    TextMeshProUGUI playTimeText;
    /// <summary>
    /// 플레이어 체력 텍스트
    /// </summary>
    TextMeshProUGUI playerHealthText;
    /// <summary>
    /// 플레이어 소지 탄약 텍스트
    /// </summary>
    TextMeshProUGUI playerAmmoText;
    /// <summary>
    /// 플레이어 소지 코인 텍스트
    /// </summary>
    TextMeshProUGUI playerCoinText;

    /// <summary>
    /// 무기이미지들
    /// </summary>
    Image weapon1Img;
    Image weapon2Img;
    Image weapon3Img;
    //Image weapon4Img;
    /// <summary>
    /// 생성되어있는 적의 수량 텍스트들
    /// </summary>
    TextMeshProUGUI enemyATxt;
    TextMeshProUGUI enemyBTxt;
    TextMeshProUGUI enemyCTxt;
    /// <summary>
    /// 보스 체력 그룹
    /// </summary>
    RectTransform bossHealthGroup;
    /// <summary>
    /// 보스 체력바
    /// </summary>
    RectTransform bossHealthBar;

    private void Awake()
    {
        //변수 세팅
        Transform child = transform.GetChild(0);
        scoreText = child.GetChild(1).GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        stageText = child.GetChild(1).GetComponent<TextMeshProUGUI>();
        playTimeText = child.GetChild(3).GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        playerHealthText = child.GetChild(1).GetComponent<TextMeshProUGUI>();
        playerAmmoText = child.GetChild(3).GetComponent <TextMeshProUGUI>();
        playerCoinText =child.GetChild(5).GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        weapon1Img = child.GetChild(1).GetChild(0).GetComponent<Image>();
        weapon2Img = child.GetChild(2).GetChild(0).GetComponent<Image>();
        weapon3Img = child.GetChild(3).GetChild(0).GetComponent<Image>();
        child = transform.GetChild(4);
        enemyATxt = child.GetChild(1).GetComponent<TextMeshProUGUI>();
        enemyBTxt = child.GetChild(3).GetComponent<TextMeshProUGUI>();
        enemyCTxt = child.GetChild(5).GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(5);
        bossHealthGroup = child.GetComponent<RectTransform>();
        bossHealthBar = child.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        
        bossHealthGroup.gameObject.SetActive(false);
        stage = 1;
    }

    private void Start()
    {
        playerInteraction = GameManager.Instance.Player.transform.GetComponent<PlayerInteraction>();
        enemyBoss = GameManager.Instance.Boss;
    }

    private void OnDisable()
    {
        isBattle =false;
        playTime = 0;
    }
    private void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
    }
    private void LateUpdate()
    {
        scoreText.text = string.Format("{0:n0}", playerInteraction.Score);
        stageText.text = $"STAGE{stage}";
        int hour = (int)(playTime / 3600);
        int miniute = (int)((playTime -hour*3600) / 60);
        int second = (int)(playTime % 60);
        playTimeText.text = $"{string.Format("{0:00}",hour)}:{string.Format("{0:00}", miniute)}:{string.Format("{0:00}", second)}";

        playerHealthText.text = $"{playerInteraction.Health} / {playerInteraction.healthMax}";
        playerCoinText.text = string.Format("{0:n0}", playerInteraction.Coin);
        if (playerInteraction.equipWeapon == null)
            playerAmmoText.text = $"- / {playerInteraction.Ammo}";
        else if (playerInteraction.equipWeapon.type == Weapon.WeaponType.Melee)
            playerAmmoText.text = $"- / {playerInteraction.Ammo}";
        else
            playerAmmoText.text = $"{playerInteraction.equipWeapon.currentAmmo} / {playerInteraction.Ammo}";

        weapon1Img.color = new Color(1, 1, 1, playerInteraction.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, playerInteraction.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, playerInteraction.hasWeapons[2] ? 1 : 0);

        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();
        if(enemyBoss!=null)
            bossHealthBar.localScale = new Vector3((float)enemyBoss.health / enemyBoss.maxHealth,1,1);
    }

    /// <summary>
    /// 다음스테이지로 넘어가는 함수
    /// </summary>
    public void NextStage()
    {
        stage++;
    }
}
