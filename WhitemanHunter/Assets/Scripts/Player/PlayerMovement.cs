using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 5.0f;

    /// <summary>
    /// 회피속도
    /// </summary>
    public float dodgeSpeed = 10.0f;

    /// <summary>
    /// 회전 정도(10이면 0.1초 정도)
    /// </summary>
    public float turnSmooth = 10.0f;

    /// <summary>
    /// 현재 이동 속도
    /// </summary>
    float currentSpeed = 1.0f;
    /// <summary>
    /// 이게 있어야 플레이어 회전 상하좌우 강제 보간을 막을 수 있음
    /// </summary>
    private bool isMove;
    /// <summary>
    /// 현재 이동 모드(기본 run)
    /// </summary>
    MoveState currentMoveMode = MoveState.Run;

    /// <summary>
    /// 플레이어가 목표로 하는 회전
    /// </summary>
    Quaternion targetRotation;

    /// <summary>
    /// 이동할 방향(3D 공간의 이동 방향, y는 무조건 바닥 높이)
    /// </summary>
    Vector3 direction = Vector3.zero;

    /// <summary>
    /// 이동할 방향을 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    public Vector3 Direction
    {
        get => direction;
        set
        {
            direction = value;
        }
    }

    /// <summary>
    /// 이동 상태 표시용 enum
    /// </summary>
    enum MoveState : byte
    {
        Stop,   // 정지 상태
        Walk,   // 걷기 상태
        Run,     // 달리기 상태
        Dodge
    }

    // 애니메이터용 해시값 및 상수
    readonly int Speed_Hash = Animator.StringToHash("Speed");
    readonly int Dodge_Hash = Animator.StringToHash("Dodge");
    const float Animator_StopSpeed = 0.0f;
    const float Animator_WalkSpeed = 0.3f;
    const float Animator_RunSpeed = 1.0f;

    // 컴포넌트
    CharacterController cc;
    Animator animator;
    private bool isDodge;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {
        SetMoveSpeedAndAnimation(MoveState.Stop);    // 일단 정지 상태
    }

    private void Update()
    {
        cc.Move(Time.deltaTime * currentSpeed * direction); // 수동
        //cc.SimpleMove(inputDirection);                    // 자동

        if (isMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSmooth);
        }
    }

    /// <summary>
    /// 이동할 방향을 지정하는 함수
    /// </summary>
    /// <param name="inputDir">입력된 방향</param>
    /// <param name="isPress">키를 눌렀으면 true, 땠으면 false</param>
    public void SetDirection(Vector2 inputDir, bool isPress)
    {
        // WASD 방향을 3차원으로 변환
        direction.x = inputDir.x;
        direction.y = 0.0f;
        direction.z = inputDir.y;

        if (isPress)
        {
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);    // 카메라의 Y축 회전만 따로 추출
            direction = camY * direction;                           // direction 방향을 camY만큼 회전 시키기
            targetRotation = Quaternion.LookRotation(direction);    // 목표로하는 회전 설정
            isMove = true;

            SetMoveSpeedAndAnimation(currentMoveMode);   // 현재 모드에 맞게 이동 속도와 애니메이션 설정
        }
        else
        {
            isMove = false;
            SetMoveSpeedAndAnimation(MoveState.Stop);    // 정지 
        }

        direction = direction.normalized;   // 정규화 시키기
    }

    /// <summary>
    /// 걷기와 달리기를 서로 토글하는 함수. (이동 모드 변화 키가 눌려질 때 실행)
    /// </summary>
    public void ToggleMoveMode()
    {
        switch( currentMoveMode )
        {
            case MoveState.Walk:                
                currentMoveMode = MoveState.Run;    // 상태 변화
                SetMoveSpeedAndAnimation(MoveState.Run);     // 속도와 애니메이션 변화
                break;
            case MoveState.Run:
                currentMoveMode = MoveState.Walk;
                SetMoveSpeedAndAnimation(MoveState.Walk);
                break;
        }
    }

    public void Dodge()
    {
        currentSpeed *= 100;
        animator.SetTrigger(Dodge_Hash);
        isDodge = true;
        Invoke("DodgeExit",0.4f);
    }

    void DodgeExit()
    {
        currentSpeed /= 100;
        isDodge=false;
    }
    /// <summary>
    /// 플레이어 이동 속도 변화와 애니메이션 처리용 함수
    /// </summary>
    /// <param name="mode">설정할 이동 모드</param>
    void SetMoveSpeedAndAnimation(MoveState mode)
    {
   
        // 속도와 애니메이션 변경
        switch (mode)
        {
            case MoveState.Stop:
                animator.SetFloat(Speed_Hash, Animator_StopSpeed);
                currentSpeed = 0.0f;
                break;
            case MoveState.Walk:
                if (direction.sqrMagnitude > 0)      // 움직일 때만 애니메이션 변경하기
                {
                    animator.SetFloat(Speed_Hash, Animator_WalkSpeed);
                }
                currentSpeed = walkSpeed;
                break;
            case MoveState.Run:
                if (direction.sqrMagnitude > 0)
                {
                    animator.SetFloat(Speed_Hash, Animator_RunSpeed);
                }
                currentSpeed = runSpeed;
                break;
            case MoveState.Dodge:
                currentSpeed = dodgeSpeed;
                break;
        }
        //Debug.Log(currentSpeed);
    }
    
}