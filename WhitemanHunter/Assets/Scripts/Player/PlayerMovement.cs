using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    /// <summary>    /// 현재 이동 속도
    /// </summary>
    float currentSpeed = 1.0f;
    /// <summary>
    /// 이게 있어야 플레이어 회전 상하좌우 강제 보간을 막을 수 있음
    /// </summary>
    public bool isMove;

    /// <summary>
    /// 정전중인지 확인용 변수
    /// </summary>
    public bool isReload;
    /// <summary>
    /// 회피중인지 확인용 변수
    /// </summary>
    private bool isDodge;

    /// <summary>
    /// 현재 이동 모드(기본 run)
    /// </summary>
    MoveState currentMoveMode = MoveState.Run;
    /// <summary>
    /// 이전 이동모드
    /// </summary>
    MoveState prevMode = MoveState.Stop;
    /// <summary>
    /// 플레이어가 목표로 하는 회전
    /// </summary>
    Quaternion targetRotation;

    /// <summary>
    /// 이동할 방향(3D 공간의 이동 방향, y는 무조건 바닥 높이)
    /// </summary>
    Vector3 direction = Vector3.zero;
    /// <summary>
    /// 플레이어 상호작용
    /// </summary>
    PlayerInteraction playerInteraction;
    /// <summary>
    /// 플레이어 공격
    /// </summary>
    PlayerAttack playerAttack;

    /// <summary>
    /// 카메라
    /// </summary>
    Camera cam;

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
    /// 회피여부를 확인할 프로퍼티
    /// </summary>
    public bool IsDodge
    {
        get => isDodge;
        set
        {
            isDodge = value;
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
    readonly int Reload_Hash = Animator.StringToHash("Reload");
    const float Animator_StopSpeed = 0.0f;
    const float Animator_WalkSpeed = 0.3f;
    const float Animator_RunSpeed = 1.0f;

    int undieLayer; 
    int playerLayer;

    // 컴포넌트
    CharacterController cc;
    Animator animator;
    Rigidbody rb;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetMoveSpeedAndAnimation(MoveState.Stop);    // 일단 정지 상태
        undieLayer = LayerMask.NameToLayer("Undie");
        playerLayer = LayerMask.NameToLayer("Player");
        
        cam =Camera.main;
    }

    private void Update()
    {
        if (!playerInteraction.isSwap && !playerAttack.isAttack && !isReload)
        {
            cc.Move(Time.deltaTime * currentSpeed * direction); // 수동
        }

        if (isMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSmooth);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TurnFromMouse(), Time.deltaTime * turnSmooth);
        }
    }
    private void FixedUpdate()
    {
        if (!cc.isGrounded)
        {
            direction.y = -10f;
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
        if (!isDodge)
        {
            switch (currentMoveMode)
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
    }

    /// <summary>
    /// 회피 함수
    /// </summary>
    public void Dodge()
    {
        if (!isDodge&&!isReload)
        {
            if (!isMove)//이동입력이 없을때 자동으로 플레이어 앞쪽으로 가게 하기
            {
                direction = transform.forward;
            }
            prevMode = currentMoveMode;
            gameObject.layer = undieLayer;
            SetMoveSpeedAndAnimation(MoveState.Dodge);
            isDodge = true;
            Invoke("DodgeExit",0.4f);
        }
    }

    /// <summary>
    /// 회피 종료 함수(딜레이를 걸어주기 위함), 타이밍 이슈로 코루틴은 회피함
    /// </summary>
    void DodgeExit()
    {
        if (!isMove)
        {
            direction = Vector3.zero;
        }
        gameObject.layer = playerLayer;
        SetMoveSpeedAndAnimation(prevMode);
        isDodge =false;
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
                if (!isDodge)
                {
                    currentSpeed = dodgeSpeed;
                    animator.SetTrigger(Dodge_Hash);
                }
                break;
        }
    }

    /// <summary>
    /// 재장전 함수
    /// </summary>
    public void Reload()
    {
        if (playerInteraction.equipWeapon == null)
            return;
        if (playerInteraction.equipWeapon.type == Weapon.WeaponType.Melee)
            return;
        if (playerInteraction.Ammo == 0)
            return;
        if (!IsDodge && !playerAttack.isAttack && !playerInteraction.isSwap)
        {
            animator.SetTrigger(Reload_Hash);
            isReload = true;
            StartCoroutine(ReloadOut());
        }
    }

    /// <summary>
    /// 마우스에 의한 회전 함수(무기가 있을 때만)
    /// </summary>
    /// <returns>마우스로의 회전방향</returns>
    public Quaternion TurnFromMouse()
    {
        if(playerInteraction.equipWeapon == null)
        {
            return targetRotation;
        }
        Quaternion result = Quaternion.identity;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0;
            result =Quaternion.LookRotation(direction);
        }
        return result;
    }

    /// <summary>
    /// 재장전 모션에서 빠져나올 때 사용할 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator ReloadOut()
    {
        Weapon weapon = playerInteraction.equipWeapon; 
        yield return new WaitForSeconds(2.0f);
        int reAmmo = playerInteraction.Ammo < weapon.maxAmmo ? playerInteraction.Ammo : weapon.maxAmmo;
        weapon.currentAmmo = reAmmo; 
        playerInteraction.Ammo -= reAmmo;
        isReload=false;
    }

    /// <summary>
    /// 순간이동 함수 (캐릭터 컨트롤러 버그 때문에 만듬)
    /// </summary>
    /// <param name="pos">이동할 위치</param>
    public void Teleport(Vector3 pos)
    {
        cc.enabled = false;
        transform.position = pos;
        cc.enabled = true;
    }
}