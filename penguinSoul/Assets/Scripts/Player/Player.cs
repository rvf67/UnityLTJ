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
    /// ���� ü��
    /// </summary>
    public int hp =100;
    public int maxHp = 100;
    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    public float moveSpeed = 0.01f;

    /// <summary>
    /// �Ѿ� �߻� ����
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// �Ѿ� ������
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// �ִ� ������ ���� ��
    /// </summary>
    public int maxJump = 2;

    /// <summary>
    /// ������ Ƚ���� �������� ����
    /// </summary>
    int jumpCnt=0;

    /// <summary>
    /// ���� ���̾��� ��ȣ
    /// </summary>
    int undieLayer;

    /// <summary>
    /// �÷��̾� ���̾��� ��ȣ
    /// </summary>
    int playerLayer;

    /// <summary>
    /// ������ ��
    /// </summary>
    public float jumpPower = 1.0f;

    /// <summary>
    /// �������� �޾��� �� ƨ�ܳ����� ��
    /// </summary>
    public float damagePow=10;
    /// <summary>
    /// �����ð�
    /// </summary>
    public  float undieTime = 2.0f;
    
    /// <summary>
    /// ù��° ������ �ڻ������� üũ
    /// </summary>
    bool isJump = false;
    
    /// <summary>
    /// �¾Ҵ��� üũ
    /// </summary>
    bool isHit =false;

    /// <summary>
    /// �ø��Ǿ����� üũ
    /// </summary>
    private bool isFlip;
    /// <summary>
    /// ������ ���� �Ҹ��� ��
    /// </summary>
    private bool isBlockedMove=false;
    /// <summary>
    /// �Էµ� ����
    /// </summary>
    Vector3 inputDirection = Vector3.zero;

    /// <summary>
    /// �Է¿� ��ǲ �׼�
    /// </summary>
    PlayerInputAction inputActions;

    /// <summary>
    /// �Ѿ� �߻�� Ʈ������
    /// </summary>
    Transform fireTransform;

    /// <summary>
    /// �Ѿ� �߻�� �ڷ�ƾ
    /// </summary>
    IEnumerator fireCoroutine;

    /// <summary>
    /// ��������Ʈ ������
    /// </summary>
    SpriteRenderer spriteRenderer;
    
    /// <summary>
    /// �ִϸ����� ������Ʈ�� ������ ����
    /// </summary>
    Animator animator;
    /// <summary>
    /// �Ѿ� �߻� ����Ʈ�� ���� ���� ������Ʈ
    /// </summary>
    GameObject fireFlashLeft;
    /// <summary>
    /// �Ѿ� �߻� ����Ʈ�� ������ ���� ������Ʈ
    /// </summary>
    GameObject fireFlashRight;

    /// <summary>
    /// �Ѿ� �߻� ����Ʈ�� ���� �ð���
    /// </summary>
    WaitForSeconds flashWait;

    /// <summary>
    /// ������ٵ� ������Ʈ
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// ������ �����ϴ� ������Ʈ
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

        inputActions = new PlayerInputAction();    // ��ǲ �׼� ����
        animator = GetComponent<Animator>();        // �ڽŰ� ���� ���ӿ�����Ʈ �ȿ� �ִ� ������Ʈ ã��        
        rigid = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        fireTransform = transform.GetChild(0);          // ù��° �ڽ� ã��
        fireFlashLeft = transform.GetChild(1).gameObject;   // �ι�° �ڽ� ã�Ƽ� �� �ڽ��� ���� ������Ʈ �����ϱ�
        fireFlashRight = transform.GetChild(2).gameObject;   // ����° �ڽ� ã�Ƽ� �� �ڽ��� ���� ������Ʈ �����ϱ�

        fireCoroutine = FireCoroutine();            // �ڷ�ƾ �����ϱ�

        flashWait = new WaitForSeconds(0.1f);       // �Ѿ� �߻�� ����Ʈ�� 0.1�� ���ȸ� ���δ�.

    }

    private void Start()
    {
        HP = maxHp;
        undieLayer = LayerMask.NameToLayer("Undie");
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnEnable()
    {
        inputActions.Enable();                          // ��ǲ �׼� Ȱ��ȭ
        inputActions.Player.Fire.performed += OnFireStart;   // �׼ǰ� �Լ� ���ε�
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
        // �׻� ���� �ð� ����(Time.fixedDeltaTime)���� ȣ��ȴ�.
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
        //�������� ���� �ִϸ��̼� ������ ���̷� �׸�
        Debug.DrawRay(rigid.position,Vector3.down, new Color(1,0,0));
        //���� ���� �ٴڿ� ��Ҵ��� Ȯ���ϴ� �ڵ�
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
    /// Move �׼��� �߻����� �� ����� �Լ�
    /// </summary>
    /// <param name="context">�Է� ����</param>
    private void OnMove(InputAction.CallbackContext value)
    {
        inputValue = value.ReadValue<Vector2>().x;   // �Է� �� �б�
    }

    /// <summary>
    /// Jump �׼��� �߻����� �� ����� �Լ�
    /// </summary>
    /// <param name="_">�Է� ����(������� �ʾƼ� ĭ�� ��Ƴ��� ��)</param>
    private void OnJump(InputAction.CallbackContext _)
    {
        if (!isBlockedMove && !isJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("Jump",true);
            Debug.Log("����!");
            isJump = true;
            jumpCnt++;
        }
        else if (!isBlockedMove && isJump &&  jumpCnt <2) //�̴�����
        {
            rigid.velocity =Vector2.zero; //velocity�� �ʱ�ȭ�Ͽ� ������ ������ ������ �ϵ��� ��
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("DoubleJump", true);
            jumpCnt++;
        }
    }

    /// <summary>
    /// Fire �׼��� �߻����� �� ����� �Լ�
    /// </summary>
    /// <param name="_">�Է� ����(������� �ʾƼ� ĭ�� ��Ƴ��� ��)</param>
    private void OnFireStart(InputAction.CallbackContext _)
    {
        StartCoroutine(fireCoroutine);
    }

    private void OnFireEnd(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);
    }

    /// <summary>
    /// �Ѿ��� �ѹ� �߻��ϴ� �Լ�
    /// </summary>
    void Fire()
    {
        // �÷��� ����Ʈ ��� �ѱ�
        StartCoroutine(FlashEffect());

        //���丮 Ȱ�� �Ѿ� ����
        Factory.Instance.GetBullet(fireTransform.position,isFlip,fireTransform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// ����� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        // �ڷ�ƾ : ���� �ð� �������� �ڵ带 �����ϰų� ���� �ð����� �����̸� �� �� ����

        while (true) // ���� ����
        {
            //Debug.Log("Fire");
            Fire();
            yield return new WaitForSeconds(fireInterval);  // fireInterval�ʸ�ŭ ��ٷȴٰ� �ٽ� �����ϱ�
        }

        // ������ ����ñ��� ���
        // yield return null;
        // yield return new WaitForEndOfFrame();
    }


    /// <summary>
    /// �߻� ����Ʈ�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashEffect()
    {
        if (isFlip)
        {
            fireTransform.position = fireFlashLeft.transform.position;
            fireFlashLeft.SetActive(true);  // ���� ������Ʈ Ȱ��ȭ�ϱ�
            yield return flashWait;     // ��� ������ �ɱ�
            fireFlashLeft.SetActive(false);
        }
        else
        {
            fireTransform.position = fireFlashRight.transform.position;
            fireFlashRight.SetActive(true);  // ���� ������Ʈ Ȱ��ȭ�ϱ�
            yield return flashWait;     // ��� ������ �ɱ�
            fireFlashRight.SetActive(false);
        }
    }

    IEnumerator Undie()
    {
        gameObject.layer = undieLayer; //���� ���̾�� ����

        float timeElapsed = 0.0f;
        while (timeElapsed < undieTime) //undieTime�� ���� �ݺ�
        {
            timeElapsed += Time.deltaTime;

            //�÷��̾� �����̱�

            //(Mathf.Cos(timeElapsed)+1.0f)*0.5f;

            //Mathf.Deg2Rad; //���ϸ� Degree�� Radian�� ��
            //Mathf.Rad2Deg; //���ϸ� Radian�� Degree�� ��

            //30.0f�� �����̴� �ӵ� ������
            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;
            spriteRenderer.color = new Color(1, 1, 1, alpha);

            yield return null; //���� �����ӱ��� ���
        }
        isHit = false;  //���� ���� ����'
        isBlockedMove = false; //������ ���� ����
        gameObject.layer = playerLayer; //�÷��̾�� ���̾� ����
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

    private void OnCollisionStay2D(Collision2D collision) //���� ���¿��� ������ ������ �浹 ������ �ȵǱ� ������STAY�� ����
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
            
            //����
            if (isCoin)
                GameManager.Instance.AddScore(100);
            else if (isBlueFlake)
                GameManager.Instance.AddScore(200);
            
            // ������ ����
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
        Debug.Log("���� �ε��ƴ�.");
        HP -= 10;
        isHit = true;
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //�浹��ġ�� ���Ͽ� ƨ�ܳ��� ���� ����
        rigid.AddForce(new Vector2(dirc, 1) * damagePow, ForceMode2D.Impulse);
        StartCoroutine(Undie());
        isBlockedMove = true;
    }
}
