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
    /// �ִ� ������ ���� ��
    /// </summary>
    public int maxJump = 1;

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
    /// ��������Ʈ ������
    /// </summary>
    SpriteRenderer spriteRenderer;
    
    /// <summary>
    /// �ִϸ����� ������Ʈ�� ������ ����
    /// </summary>
    Animator animator;


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
            isJump = true;
        }
  
    }

    private void OnDodge(InputAction.CallbackContext _)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// �������̾� ����
    /// </summary>
    /// <returns></returns>
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHit && collision.gameObject.CompareTag("Enemy"))
        {
            Hit(collision.transform.position);
        }
    }


    /// <summary>
    /// ������ �¾��� �� ó��
    /// </summary>
    /// <param name="decreaseHp">UIü�°��Ҿ�</param>
    public void OnHit(float decreaseHp)
    {
        gameManager.DecreaseHp(decreaseHp);
        animator.SetBool("Idle", false);
        animator.SetTrigger("DamageTrigger");
        animator.SetBool("Jump", false);
        animator.SetFloat("Walk", 0.0f);
    }

    /// <summary>
    /// �¾��� �� ƨ�ܳ��� ��������, ���� ü�°��ҵ� ��
    /// </summary>
    /// <param name="targetPos">�浹��ġ</param>
    private void Hit(Vector2 targetPos)
    {
        HP -= 10;
        isHit = true;
        StartCoroutine(Undie());
    }


}
