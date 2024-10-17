using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� Ŭ������ ���׸� Ÿ���̴�.
// �� Ŭ������ TŸ���� component�� ��ӹ��� Ÿ�Ը� �����ϴ�.
public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// �� �̱����� �ʱ�ȭ ������ �ִ����� ����ϴ� ����
    /// </summary>
    private bool isInitialize = false;

    /// <summary>
    /// ����ó���� ������ Ȯ�ο� ����(static �ɹ� �Լ� �ȿ����� static �ɹ� ������ ���� ����)
    /// </summary>
    private static bool isShutdown = false;

    /// <summary>
    /// �� �̱����� �ν��Ͻ�(new �Ѱ�)
    /// </summary>
    private static T instance = null;

    /// <summary>
    /// �� �̱��濡 �����ϱ� ���� ������Ƽ
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutdown)  // ����ó���� �� ��Ȳ�̸�
            {
                Debug.LogWarning("�̱����� ���� �߿� �䱸����.");     // ��� ���
                return null;                                        // null ����
            }

            if (instance == null)
            {
                T singleton = FindAnyObjectByType<T>();     // �ϴ� ���� �̱����� �ִ��� ã��
                if (singleton == null)
                {
                    // ���� �̱����� ����
                    GameObject obj = new GameObject();      // Transform �ϳ��� ����ִ� �� ���� ������Ʈ �����
                    obj.name = $"{typeof(T)}_Singleton";    // �̱��� ���� ������Ʈ�� �̸� �����ϱ�
                    singleton = obj.AddComponent<T>();      // �̱��� ���� ������Ʈ�� �̱��� �� ������Ʈ �߰�
                }
                instance = singleton;       // ã�� ���̳� ���� ���� �����ϱ�
                DontDestroyOnLoad(instance.gameObject);     // ���� �������� �̱���� ���ӿ�����Ʈ�� �����ϱ�(�������� �ʰ� �ϱ�)
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            // ù��°�� ������� �̱���
            instance = this as T;       // this�� TŸ������ ĳ����. ĳ������ �ȵǸ� null
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            // �̹� ������� �̱����� �ִ� ��Ȳ
            if (instance != this)           // �̹� ������� ���� �ڽ��� �ƴϸ�
            {
                Destroy(this.gameObject);   // ������ ��������
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;      // ���� �ε��Ǿ��� �� OnSceneLoaded�Լ��� �����ϵ��� ���
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;      // ��� ����
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isInitialize)
        {
            // �ѹ��� �ʱ�ȭ ���� �ʾ�����
            OnPreInitialize();      // �ѹ��� �����ϴ� �ʱ�ȭ �Լ� ����
        }
        if (mode != LoadSceneMode.Additive)
        {
            // Additive���� ���� �ε����� �ʾҴٸ�
            OnInitialize();         // �ݺ� �ʱ�ȭ �Լ� ����
        }
    }

    /// <summary>
    /// �̱����� ��������� �� �� �ѹ��� ȣ��Ǵ� �Լ�
    /// </summary>
    protected virtual void OnPreInitialize()
    {
        isInitialize = true;
    }

    /// <summary>
    /// �̱����� ��������� ���� ����� ������ ȣ��� �Լ�(Additive�� �ȵ�)
    /// </summary>
    protected virtual void OnInitialize()
    {
    }

    private void OnApplicationQuit()
    {
        isShutdown = true;
    }
}


// �̱��� : �����Ǵ� ��ü�� ������ 1���� Ŭ����
public class TestSingleton
{
    private static TestSingleton instance = null;  // �������� ���Ǵ� ���� �ν��Ͻ��� ������ ����

    public static TestSingleton Instance
    {
        get
        {
            if (instance == null)    // ������ �ν��Ͻ��� ������� ���� ������
            {
                instance = new TestSingleton(); // �ν��Ͻ� ����
            }
            return instance;        // �ִ� ���̳� ���� ������� ���� ����
        }
    }

    private TestSingleton()  // ��� �����ڴ� public�� �ƴϾ�� �Ѵ�.
    {
    }
}

public class TestClass
{
    // ����ƽ �ɹ� ������ ��� ��ü���� �������� ���ȴ�.
    public static int staticNumber = 0;

    // ������ : ��ü�� ������ ��(new �ɶ�) ����Ǵ� �ڵ�
    // �����ڴ� �������� ����� �ִ�.(�Ķ���ʹ� �޶����)
    // �����ڸ� �ϳ��� �ۼ����� ������ �⺻�����ڰ� �ڵ����� �����ȴ�.
    public TestClass()  // �⺻������
    {
        Debug.Log("TestClass ������ ����");
    }

    public TestClass(int i)
    {
        Debug.Log($"TestClass ������ ���� : {i}");
    }

    public void TestPrint()
    {
        Debug.Log(staticNumber);
    }
}