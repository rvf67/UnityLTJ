using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PenguinController : MonoBehaviour
{
    [SerializeField]
    float inputValue;
    float moveValue;
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
    /// 점프의 힘
    /// </summary>
    public float jumpPower = 1.0f;

    bool isJump = false;
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
    /// 총알 발사 이팩트용 게임 오브젝트
    /// </summary>
    GameObject fireFlash;

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
    GameDirector gameDirector;


    private void Awake()
    {

        inputActions = new PlayerInputAction();    // 인풋 액션 생성

        animator = GetComponent<Animator>();        // 자신과 같은 게임오브젝트 안에 있는 컴포넌트 찾기        
        rigid = GetComponent<Rigidbody2D>();
        gameDirector = GetComponent<GameDirector>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        fireTransform = transform.GetChild(0);          // 첫번째 자식 찾기
        fireFlash = transform.GetChild(1).gameObject;   // 두번째 자식 찾아서 그 자식의 게임 오브젝트 저장하기

        fireCoroutine = FireCoroutine();            // 코루틴 저장하기

        flashWait = new WaitForSeconds(0.1f);       // 총알 발사용 이팩트는 0.1초 동안만 보인다.
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
        rigid.velocity = new Vector2(moveValue,rigid.velocity.y);
        if (rigid.velocity.x != 0)
        {
            spriteRenderer.flipX = rigid.velocity.x < 0;
            animator.SetFloat("Walk", 1.0f);
        }
        else
        {
            animator.SetFloat("Walk", 0.0f);
        }

        //점프 도중 바닥에 닿았는지 확인하는 코드
        //RaycastHit2D rayHit = Physics2D.Raycast
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
        if (!isJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("Jump",true);
            Debug.Log("점프!");
            isJump = true;
            jumpCnt++;
        }
        else if (isJump &&  jumpCnt <2) //이단점프
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
        //Debug.Log("발사 시작");
        //Fire();
        //StartCoroutine("FireCoroutine");
        //StartCoroutine(FireCoroutine());
        StartCoroutine(fireCoroutine);
    }

    private void OnFireEnd(InputAction.CallbackContext _)
    {
        //Debug.Log("발사 종료");
        //StopAllCoroutines();    // 모든 코루틴 정지시키기
        //StopCoroutine("FireCoroutine");
        //StopCoroutine(FireCoroutine());
        StopCoroutine(fireCoroutine);
    }

    /// <summary>
    /// 총알을 한발 발사하는 함수
    /// </summary>
    void Fire()
    {
        // 플래시 이팩트 잠깐 켜기
        StartCoroutine(FlashEffect());

        // 총알 생성
        //Instantiate(bulletPrefab, transform); // 자식은 부모를 따라다니므로 이렇게 하면 안됨
        //Instantiate(bulletPrefab, transform.position, Quaternion.identity);           // 총알이 비행기와 같은 위치에 생성
        //Instantiate(bulletPrefab, transform.position + Vector3.right, Quaternion.identity);   // 총알 발사 위치를 확인하기 힘듬
        //Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);  // 총알을 fireTransform의 위치와 회전에 따라 생성

        //팩토리 활용 총알 생성
        //Factory.Instance.GetBullet(fireTransform.position, fireTransform.rotation.eulerAngles.z);
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
        fireFlash.SetActive(true);  // 게임 오브젝트 활성화하기
        yield return flashWait;     // 잠깐 딜레이 걸기
        fireFlash.SetActive(false);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // 이쪽을 권장. ==에 비해 가비지가 덜 생성된다. 생성되는 코드도 훨씬 빠르게 구현되어 있음.
        {
            Debug.Log("적과 부딪쳤다.");
            gameDirector.DecreaseHp();
            this.animator.SetTrigger("DamageTrigger");
        }
 
    }
}
