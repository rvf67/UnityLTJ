using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    float inputValue;
    float moveValue;

    /// <summary>
    /// 현재 체력
    /// </summary>
    public int hp =100;
    public int maxHp = 100;
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 0.01f;

    /// <summary>
    /// 최대 가능한 점프 수
    /// </summary>
    public int maxJump = 1;

    /// <summary>
    /// 무적 레이어의 번호
    /// </summary>
    int undieLayer;

    /// <summary>
    /// 플레이어 레이어의 번호
    /// </summary>
    int playerLayer;

    /// <summary>
    /// 점프의 힘
    /// </summary>
    public float jumpPower = 1.0f;

    /// <summary>
    /// 무적시간
    /// </summary>
    public  float undieTime = 2.0f;
    
    /// <summary>
    /// 첫번째 점프를 뛴상태인지 체크
    /// </summary>
    bool isJump = false;
    
    /// <summary>
    /// 맞았는지 체크
    /// </summary>
    bool isHit =false;

    /// <summary>
    /// 플립되었는지 체크
    /// </summary>
    private bool isFlip;
    /// <summary>
    /// 움직임 막는 불리언 값
    /// </summary>
    private bool isBlockedMove=false;
    /// <summary>
    /// 입력된 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;

    /// <summary>
    /// 입력용 인풋 액션
    /// </summary>
    PlayerInputAction inputActions;

    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer spriteRenderer;
    
    /// <summary>
    /// 애니메이터 컴포넌트를 저장할 변수
    /// </summary>
    Animator animator;


    /// <summary>
    /// 리지드바디 컴포넌트
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// 게임을 감독하는 컴포넌트
    /// </summary>
    GameManager gameManager;

    public int HP
    {
        get => hp;

        set
        {
            if (hp != value)
            {
                hp = value;
                OnHit((float)HP/maxHp);
                hp = Mathf.Clamp(hp, 0, maxHp);
            }
        }
    }


    private void Awake()
    {

        inputActions = new PlayerInputAction();    // 인풋 액션 생성
        animator = GetComponent<Animator>();        // 자신과 같은 게임오브젝트 안에 있는 컴포넌트 찾기        
        rigid = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    private void Start()
    {
        HP = maxHp;
        undieLayer = LayerMask.NameToLayer("Undie");
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnEnable()
    {
        inputActions.Enable();                          // 인풋 액션 활성화
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Dodge.performed += OnDodge;
    }

    

    private void OnDisable()
    {
        inputActions.Player.Dodge.performed -= OnDodge;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Disable();
    }


    private void FixedUpdate()
    {
        // 항상 일정 시간 간격(Time.fixedDeltaTime)으로 호출된다.
        // Debug.Log(Time.fixedDeltaTime);

        moveValue = inputValue * moveSpeed;
        if (!isBlockedMove)
        {
            rigid.velocity = new Vector2(moveValue, rigid.velocity.y);
            if (rigid.velocity.x != 0)
            {
                spriteRenderer.flipX = rigid.velocity.x < 0;
                isFlip = rigid.velocity.x < 0;
                animator.SetFloat("Walk", 1.0f);
            }
            else
            {
                animator.SetFloat("Walk", 0.0f);
            }
        }
        //점프도중 점프 애니메이션 범위를 레이로 그림
        Debug.DrawRay(rigid.position,Vector3.down, new Color(1,0,0));
        //점프 도중 바닥에 닿았는지 확인하는 코드
        if (rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.0f,LayerMask.GetMask("Map"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    animator.SetBool("Jump", false);
                    animator.SetFloat("Walk", 0.0f);
                    animator.SetBool("Idle",true);
                    isJump = false;
                    isBlockedMove = false;
                }
            }
        
        }
    }

    /// <summary>
    /// Move 액션이 발생했을 때 실행될 함수
    /// </summary>
    /// <param name="context">입력 정보</param>
    private void OnMove(InputAction.CallbackContext value)
    {
        inputValue = value.ReadValue<Vector2>().x;   // 입력 값 읽기
    }

    /// <summary>
    /// Jump 액션이 발생했을 때 실행될 함수
    /// </summary>
    /// <param name="_">입력 정보(사용하지 않아서 칸만 잡아놓은 것)</param>
    private void OnJump(InputAction.CallbackContext _)
    {
        if (!isBlockedMove && !isJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("Jump",true);
            isJump = true;
        }
  
    }

    private void OnDodge(InputAction.CallbackContext _)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 무적레이어 지정
    /// </summary>
    /// <returns></returns>
    IEnumerator Undie()
    {
        gameObject.layer = undieLayer; //무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < undieTime) //undieTime초 동안 반복
        {
            timeElapsed += Time.deltaTime;

            //플레이어 깜빡이기

            //(Mathf.Cos(timeElapsed)+1.0f)*0.5f;

            //Mathf.Deg2Rad; //곱하면 Degree가 Radian이 됨
            //Mathf.Rad2Deg; //곱하면 Radian이 Degree가 됨

            //30.0f는 깜빡이는 속도 증폭율
            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;
            spriteRenderer.color = new Color(1, 1, 1, alpha);

            yield return null; //다음 프레임까지 대기
        }
        isHit = false;  //명중 여부 리셋'
        isBlockedMove = false; //움직임 막기 리셋
        gameObject.layer = playerLayer; //플레이어로 레이어 복구
        animator.SetBool("Idle", true);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHit && collision.gameObject.CompareTag("Enemy"))
        {
            Hit(collision.transform.position);
        }
    }


    /// <summary>
    /// 적에게 맞았을 때 처리
    /// </summary>
    /// <param name="decreaseHp">UI체력감소양</param>
    public void OnHit(float decreaseHp)
    {
        gameManager.DecreaseHp(decreaseHp);
        animator.SetBool("Idle", false);
        animator.SetTrigger("DamageTrigger");
        animator.SetBool("Jump", false);
        animator.SetFloat("Walk", 0.0f);
    }

    /// <summary>
    /// 맞았을 때 튕겨나갈 방향지정, 실제 체력감소도 함
    /// </summary>
    /// <param name="targetPos">충돌위치</param>
    private void Hit(Vector2 targetPos)
    {
        HP -= 10;
        isHit = true;
        StartCoroutine(Undie());
    }


}
