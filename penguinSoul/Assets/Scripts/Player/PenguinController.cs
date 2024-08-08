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
    /// ������ ��
    /// </summary>
    public float jumpPower = 1.0f;

    bool isJump = false;
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
    /// �Ѿ� �߻� ����Ʈ�� ���� ������Ʈ
    /// </summary>
    GameObject fireFlash;

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
    GameDirector gameDirector;


    private void Awake()
    {

        inputActions = new PlayerInputAction();    // ��ǲ �׼� ����

        animator = GetComponent<Animator>();        // �ڽŰ� ���� ���ӿ�����Ʈ �ȿ� �ִ� ������Ʈ ã��        
        rigid = GetComponent<Rigidbody2D>();
        gameDirector = GetComponent<GameDirector>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        fireTransform = transform.GetChild(0);          // ù��° �ڽ� ã��
        fireFlash = transform.GetChild(1).gameObject;   // �ι�° �ڽ� ã�Ƽ� �� �ڽ��� ���� ������Ʈ �����ϱ�

        fireCoroutine = FireCoroutine();            // �ڷ�ƾ �����ϱ�

        flashWait = new WaitForSeconds(0.1f);       // �Ѿ� �߻�� ����Ʈ�� 0.1�� ���ȸ� ���δ�.
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

        //���� ���� �ٴڿ� ��Ҵ��� Ȯ���ϴ� �ڵ�
        //RaycastHit2D rayHit = Physics2D.Raycast
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
        if (!isJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("Jump",true);
            Debug.Log("����!");
            isJump = true;
            jumpCnt++;
        }
        else if (isJump &&  jumpCnt <2) //�̴�����
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
        //Debug.Log("�߻� ����");
        //Fire();
        //StartCoroutine("FireCoroutine");
        //StartCoroutine(FireCoroutine());
        StartCoroutine(fireCoroutine);
    }

    private void OnFireEnd(InputAction.CallbackContext _)
    {
        //Debug.Log("�߻� ����");
        //StopAllCoroutines();    // ��� �ڷ�ƾ ������Ű��
        //StopCoroutine("FireCoroutine");
        //StopCoroutine(FireCoroutine());
        StopCoroutine(fireCoroutine);
    }

    /// <summary>
    /// �Ѿ��� �ѹ� �߻��ϴ� �Լ�
    /// </summary>
    void Fire()
    {
        // �÷��� ����Ʈ ��� �ѱ�
        StartCoroutine(FlashEffect());

        // �Ѿ� ����
        //Instantiate(bulletPrefab, transform); // �ڽ��� �θ� ����ٴϹǷ� �̷��� �ϸ� �ȵ�
        //Instantiate(bulletPrefab, transform.position, Quaternion.identity);           // �Ѿ��� ������ ���� ��ġ�� ����
        //Instantiate(bulletPrefab, transform.position + Vector3.right, Quaternion.identity);   // �Ѿ� �߻� ��ġ�� Ȯ���ϱ� ����
        //Instantiate(bulletPrefab, fireTransform.position, fireTransform.rotation);  // �Ѿ��� fireTransform�� ��ġ�� ȸ���� ���� ����

        //���丮 Ȱ�� �Ѿ� ����
        //Factory.Instance.GetBullet(fireTransform.position, fireTransform.rotation.eulerAngles.z);
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
        fireFlash.SetActive(true);  // ���� ������Ʈ Ȱ��ȭ�ϱ�
        yield return flashWait;     // ��� ������ �ɱ�
        fireFlash.SetActive(false);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // ������ ����. ==�� ���� �������� �� �����ȴ�. �����Ǵ� �ڵ嵵 �ξ� ������ �����Ǿ� ����.
        {
            Debug.Log("���� �ε��ƴ�.");
            gameDirector.DecreaseHp();
            this.animator.SetTrigger("DamageTrigger");
        }
 
    }
}
