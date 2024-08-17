using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    /// <summary>
    /// 점수가 올라가는 최소 속도(초당)
    /// </summary>
    public float scoreUpMinSpeed = 50.0f;

    [Range(1.0f, 10.0f)]    // 변수의 범위를 일정 범위 안으로 조정할 수 있게 해주는 attribute
    /// <summary>
    /// 점수 증가 속도 변경용
    /// </summary>
    public float scoreUpSpeedModifier = 5.0f; 

    /// <summary>
    /// 점수 출력용 UI
    /// </summary>
    TextMeshProUGUI score;

    /// <summary>
    /// 실제 점수
    /// </summary>
    int goalScore = 0;

    /// <summary>
    /// 보이는 점수
    /// </summary>
    float displayScore = 0.0f;

    /// <summary>
    /// 점수 확인용 프로퍼티(읽기 전용)
    /// </summary>
    public int Score
    {
        get => goalScore;
        private set     // private에서는 설정 가능
        {
            goalScore = value;
            //score.text = $"Score : {goalScore,5}";    // 5자리로 출력, 공백은 스페이스
            //score.text = $"Score : {goalScore:d5}";     // 5자리로 출력, 공백은 0으로 채우기
            //score.text = $"{goalScore}";
        }
    }

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        score = child.GetComponent<TextMeshProUGUI>();

        //GetComponents<TestBase>();    // 이 게임 오브젝트에 들어있는 모든 TestBase 찾기
        //TextMeshProUGUI[] result = GetComponentsInChildren<TextMeshProUGUI>();    // 자신과 자신의 모든 자식에 들어있는 TextMeshProUGUI 찾기
    }

    // 실습
    // 1. 점수가 바로 적용되는 것이 아니라 천천히 증가되게 만들어보기
    // 2. 보이는 점수와 실제 점수의 차이가 크면 클수록 빠르게 증가한다.
    void Update()
    {
        // displayScore가 goalScore로 될 때까지 계속 증가시키기
        if (displayScore < goalScore) 
        {
            // displayScore가 goalScore보다 작다.

            // 증가 속도 결정(goalScore와 displayScore의 차이가 크면 클수록 빠르게 증가, 최소치는 scoreUpMinSpeed)
            float speed = Mathf.Max( (goalScore - displayScore) * scoreUpSpeedModifier, scoreUpMinSpeed );

            displayScore += Time.deltaTime * speed; // 속도에 따라 displayScore 증가

            displayScore = Mathf.Min(displayScore, goalScore);  // displayScore가 goalScore를 넘지 못하게 설정

            // UI에 출력하기
            //score.text = displayScore.ToString(); // 아래와 같은 코드
            //score.text = $"{displayScore}";
            //score.text = $"{(int)displayScore}";    // 캐스팅으로 소수점 제거하기
            //score.text = displayScore.ToString("f0"); // 아래와 같은 코드
            score.text = $"{displayScore:f0}";          // 소수점 제거하기(포맷으로 변경)
        }
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void OnInitialize()
    {
        Score = 0;
        score.text = $"0";
    }

    /// <summary>
    /// 점수를 증가시키는 함수
    /// </summary>
    /// <param name="point">증가시킬 양</param>
    public void AddScore(int point)
    {
        Score += point;
    }
}
