using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    /// <summary>
    /// ���� ��������
    /// </summary>
    int stage;
    public int Stage
    {
        get => stage;
    }
    /// <summary>
    /// ���� �÷��̽ð�
    /// </summary>
    float playTime;
    /// <summary>
    /// ��Ʋ ������ 
    /// </summary>
    public bool isBattle;
    /// <summary>
    /// ���� ī��Ʈ��
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
    /// �÷��̾� ��ȣ�ۿ�
    /// </summary>
    PlayerInteraction playerInteraction;
    /// <summary>
    /// ����
    /// </summary>
    EnemyBoss enemyBoss;
    /// <summary>
    /// ���� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI scoreText;
    /// <summary>
    /// �������� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI stageText;
    /// <summary>
    /// Ǯ����Ÿ�� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI playTimeText;
    /// <summary>
    /// �÷��̾� ü�� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI playerHealthText;
    /// <summary>
    /// �÷��̾� ���� ź�� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI playerAmmoText;
    /// <summary>
    /// �÷��̾� ���� ���� �ؽ�Ʈ
    /// </summary>
    TextMeshProUGUI playerCoinText;

    /// <summary>
    /// �����̹�����
    /// </summary>
    Image weapon1Img;
    Image weapon2Img;
    Image weapon3Img;
    //Image weapon4Img;
    /// <summary>
    /// �����Ǿ��ִ� ���� ���� �ؽ�Ʈ��
    /// </summary>
    TextMeshProUGUI enemyATxt;
    TextMeshProUGUI enemyBTxt;
    TextMeshProUGUI enemyCTxt;
    /// <summary>
    /// ���� ü�� �׷�
    /// </summary>
    RectTransform bossHealthGroup;
    /// <summary>
    /// ���� ü�¹�
    /// </summary>
    RectTransform bossHealthBar;

    private void Awake()
    {
        //���� ����
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
    /// �������������� �Ѿ�� �Լ�
    /// </summary>
    public void NextStage()
    {
        stage++;
    }
}
