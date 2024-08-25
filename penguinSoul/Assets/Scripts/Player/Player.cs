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
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// 총알 프리팹
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// 최대 가능한 점프 수
    /// </summary>
    public int maxJump = 2;

    /// <summary>
    /// 점프한 횟수를 세기위한 변수
    /// </summary>
    int jumpCnt=0;

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
    /// 데미지를 받았을 때 튕겨나가는 힘
    /// </summary>
    public float damagePow=10;
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
    /// 총알 발사용 트랜스폼
    /// </summary>
    Transform fireTransform;

    /// <summary>
    /// 총알 발사용 코루틴
    /// </summary>
    IEnumerator fireCoroutine;

    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer spriteRenderer;
    
    /// <summary>
    /// 애니메이터 컴포넌트를 저장할 변수
    /// </summary>
    Animator animator;
    /// <summary>
    /// 총알 발사 이팩트용 왼쪽 게임 오브젝트
    /// </summary>
    GameObject fireFlashLeft;
    /// <summary>
    /// 총알 발사 이팩트용 오른쪽 게임 오브젝트
    /// </summary>
    GameObject fireFlashRight;

    /// <summary>
    /// 총알 발사 이팩트가 보일 시간용
    /// </summary>
    WaitForSeconds flashWait;

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
        
        fireTransform = transform.GetChild(0);          // 첫번째 자식 찾기
        fireFlashLeft = transform.GetChild(1).gameObject;   // 두번째 자식 찾아서 그 자식의 게임 오브젝트 저장하기
        fireFlashRight = transform.GetChild(2).gameObject;   // 세번째 자식 찾아서 그 자식의 게임 오브젝트 저장하기

        fireCoroutine = FireCoroutine();            // 코루틴 저장하기

        flashWait = new WaitForSeconds(0.1f);       // 총알 발사용 이팩트는 0.1초 동안만 보인다.

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
        inputActions.Player.Fire.performed += OnFireStart;   // 액션과 함수 바인딩
        inputActions.Player.Fire.canceled += OnFireEnd;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Fire.canceled -= OnFireEnd;
        inputActions.Player.Fire.performed -= OnFireStart;
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
                    jumpCnt = 0;
                    animator.SetBool("DoubleJump", false);
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
            Debug.Log("점프!");
            isJump = true;
            jumpCnt++;
        }
        else if (!isBlockedMove && isJump &&  jumpCnt <2) //이단점프
        {
            rigid.velocity =Vector2.zero; //velocity를 초기화하여 일정한 높이의 점프를 하도록 함
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("DoubleJump", true);
            jumpCnt++;
        }
    }

    /// <summary>
    /// Fire 액션이 발생했을 때 실행될 함수
    /// </summary>
    /// <param name="_">입력 정보(사용하지 않아서 칸만 잡아놓은 것)</param>
    private void OnFireStart(InputAction.CallbackContext _)
    {
        StartCoroutine(fireCoroutine);
    }

    private void OnFireEnd(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);
    }

    /// <summary>
    /// 총알을 한발 발사하는 함수
    /// </summary>
    void Fire()
    {
        // 플래시 이팩트 잠깐 켜기
        StartCoroutine(FlashEffect());

        //팩토리 활용 총알 생성
        Factory.Instance.GetBullet(fireTransform.position,isFlip,fireTransform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// 연사용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        // 코루틴 : 일정 시간 간격으로 코드를 실행하거나 일정 시간동안 딜레이를 줄 때 유용

        while (true) // 무한 루프
        {
            //Debug.Log("Fire");
            Fire();
            yield return new WaitForSeconds(fireInterval);  // fireInterval초만큼 기다렸다가 다시 시작하기
        }

        // 프레임 종료시까지 대기
        // yield return null;
        // yield return new WaitForEndOfFrame();
    }


    /// <summary>
    /// 발사 이팩트용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashEffect()
    {
        if (isFlip)
        {
            fireTransform.position = fireFlashLeft.transform.position;
            fireFlashLeft.SetActive(true);  // 게임 오브젝트 활성화하기
            yield return flashWait;     // 잠깐 딜레이 걸기
            fireFlashLeft.SetActive(false);
        }
        else
        {
            fireTransform.position = fireFlashRight.transform.position;
            fireFlashRight.SetActive(true);  // 게임 오브젝트 활성화하기
            yield return flashWait;     // 잠깐 딜레이 걸기
            fireFlashRight.SetActive(false);
        }
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isHit && collision.gameObject.CompareTag("Enemy"))
        {
            Hit(collision.transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) //맞은 상태에서 적위에 있으면 충돌 구현이 안되기 때문에STAY도 넣음
    {
        if (!isHit && collision.gameObject.CompareTag("Enemy"))
        {
            Hit(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            bool isCoin = collision.gameObject.name.Contains("coin");
            bool isBlueFlake = collision.gameObject.name.Contains("blue");
            
            //점수
            if (isCoin)
                GameManager.Instance.AddScore(100);
            else if (isBlueFlake)
                GameManager.Instance.AddScore(200);
            
            // 아이템 삭제
            collision.gameObject.SetActive(false);
        }
    }

    public void OnHit(float decreaseHp)
    {
        gameManager.DecreaseHp(decreaseHp);
        animator.SetBool("Idle", false);
        animator.SetTrigger("DamageTrigger");
        animator.SetBool("Jump", false);
        animator.SetBool("DoubleJump", false);
        animator.SetFloat("Walk", 0.0f);

    }

    private void Hit(Vector2 targetPos)
    {
        Debug.Log("적과 부딪쳤다.");
        HP -= 10;
        isHit = true;
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //충돌위치를 비교하여 튕겨나갈 방향 조절
        rigid.AddForce(new Vector2(dirc, 1) * damagePow, ForceMode2D.Impulse);
        StartCoroutine(Undie());
        isBlockedMove = true;
    }
}
