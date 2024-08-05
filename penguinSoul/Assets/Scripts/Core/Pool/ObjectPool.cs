using System.Collections.Generic;
using UnityEngine;

// ������Ʈ Ǯ
// Ư�� ������ ������Ʈ(prefab)�� ���� ������ �뷮(poolSize)���� �����ϰ� ��û�޾��� �� �ϳ��� �����ϴ� Ŭ����
public class ObjectPool<T> : MonoBehaviour where T : RecycleObject  // T�� �ݵ�� RecycleObject�� ��ӹ��� ������Ʈ��
{
    /// <summary>
    /// Ǯ���� ������ ���� ������Ʈ�� ������
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// Ǯ�� ũ��. ó���� �����ϴ� ������Ʈ�� ����(ũ��� 2^n�� ����)
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// ������ ��� ������Ʈ�� ����ִ� �迭.(���׸�(TŸ��)���� �ؼ� �پ��� ������Ʈ�� ����)
    /// </summary>
    T[] pool;

    /// <summary>
    /// ���� ��밡���� ������Ʈ���� �����ϴ� ť(pool�迭���� ��Ȱ��ȭ �Ǿ� �ִ� ������Ʈ�� ����ִ� �ڷᱸ��)
    /// </summary>
    Queue<T> readyQueue;


    /// <summary>
    /// �ʱ�ȭ�� �Լ�
    /// </summary>
    public virtual void Initialize()
    {
        if (pool == null)
        {
            // ������ ó�� Ǯ�� ����� ���. Ǯ ����
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);    // �ڷᱸ���� ����� �� �ִ� ũ�⸦ �˰� �ִٸ� capacity�� �����ϴ� ���� ����.

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            // Ǯ�� �̹� ������� �ִ� ��Ȳ(���� �ٽ� ���۵ǰų�, �ٸ� ���� �߰��� �ε��ǰų� ���)
            foreach (T obj in pool)
            {
                obj.gameObject.SetActive(false);    // ���� ��Ȱ��ȭ
            }
        }
    }

    /// <summary>
    /// Ǯ���� ����� ������Ʈ���� �����ϴ� �Լ�
    /// </summary>
    /// <param name="start">���� ���� ������ �ε���</param>
    /// <param name="end">���� ������ ������ �ε���+1</param>
    /// <param name="result">������ ������Ʈ�� �� �迭(�Է� �� ��¿�)</param>
    void GenerateObjects(int start, int end, T[] result)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(prefab, transform);    // pool�� �ڽ����� ������Ʈ ����
            obj.name = $"{prefab.name}_{i}";    // �̸� ����(�����̿��ؼ� �����ϱ�)

            T comp = obj.GetComponent<T>();
            comp.onDisable += () =>         // �����Լ��� comp�� onDisable ��������Ʈ�� ��� = comp�� ��Ȱ��ȭ �Ǹ� �Ʒ� �ڵ尡 ����ȴ�.
            {
                readyQueue.Enqueue(comp);   // ����ť�� ������Ʈ �߰��� ����
            };
            OnGenerateObject(comp);

            result[i] = comp;       // �迭�� ������� ���� ��� ����
            obj.SetActive(false);   // ��Ȱ��ȭ ��Ű��
        }
    }

    /// <summary>
    /// ������Ʈ �ϳ��� �����Ǿ��� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="comp">������ ������Ʈ ������Ʈ</param>
    protected virtual void OnGenerateObject(T comp)
    {

    }
    //void DisableAction()
    //{
    //    readyQueue.Enqueue(comp); // �������� ���� �ʴ�
    //}

    /// <summary>
    /// Ǯ���� ��Ȱ������ ������Ʈ�� �ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="position">��ġ�� ��ġ(������ǥ)</param>
    /// <param name="eulerAngle">�ʱ� ȸ��</param>
    /// <returns>Ȱ��ȭ�� ������Ʈ</returns>
    public T GetObject(Vector3? position = null, Vector3? eulerAngle = null)
    {
        if (readyQueue.Count > 0)
        {
            // ���� ��Ȱ��ȭ �� ������Ʈ�� �����ִ�.
            T comp = readyQueue.Dequeue();          // ť���� �ϳ� ������
            comp.gameObject.SetActive(true);        // Ȱ��ȭ ��Ű��
            comp.transform.position = position.GetValueOrDefault();                      // ��ġ�� ȸ�� ����
            comp.transform.rotation = Quaternion.Euler(eulerAngle.GetValueOrDefault());
            return comp;    // ����
        }
        else
        {
            // ��� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִ�. => �����ִ� ������Ʈ�� ����.
            ExpandPool();                           // Ǯ�� �ι�� �ø���
            return GetObject(position, eulerAngle); // ���Ӱ� ������ ��û
        }

    }

    /// <summary>
    /// Ǯ�� �ι�� Ȯ���Ű�� �Լ�
    /// </summary>
    void ExpandPool()
    {
        // �ִ��� ������� �ʾƾ� ��. ���� �� ���Ǹ� ���� �Լ�
        Debug.LogWarning($"{gameObject.name} Ǯ ������ ����. {poolSize} -> {poolSize * 2}");

        int newSize = poolSize * 2;         // �� Ǯ�� ũ�� ����
        T[] newPool = new T[newSize];       // �� Ǯ�� �迭 �����
        for (int i = 0; i < poolSize; i++)
        {
            newPool[i] = pool[i];           // ���� Ǯ�� ������ �� Ǯ�� ����
        }

        GenerateObjects(poolSize, newSize, newPool);    // �� Ǯ�� ���� �κп� ������Ʈ �����ؼ� �߰�

        pool = newPool;     // �� Ǯ ũ�� ����
        poolSize = newSize; // �� Ǯ�� ���� Ǯ�� ����
    }

}
