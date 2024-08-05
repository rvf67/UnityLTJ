using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MultiSpawner : MonoBehaviour
{
    public enum SpawnType
    {
        Shark = 0,
    }

    //����ȭ : Ư�� �����Ͱ� �޸𸮻� ���������� �ٰ� �ϴ� �۾�
    [Serializable] //�Ʒ� Ŭ������ ����ȭ�ؼ� �����ϰڴٴ� �ǹ�.
                   //�ν����� â���� ��� ������ ����ü�� Ŭ���� ���θ� Ȯ���ϰ� ���� �� �ݵ�� �߰��ؾ� �Ѵ�.
    public struct SpawnData
    {
        public SpawnType type;
        public float interval;
    }

    protected const float MinY = -4;
    protected const float MaxY = 4;

    /// <summary>
    /// ������(�������� �ʿ��� ����)
    /// </summary>
    Transform destinationArea;
    /// <summary>
    /// ������ ������ ���� ���� ������ ������ ���� �迭
    /// </summary>
    public SpawnData[] spawnDatas;

    private void Awake()
    {
        destinationArea = transform.GetChild(0);
    }
    private void Start()
    {
        foreach (var data in spawnDatas)
        {
            StartCoroutine(SpawnCoroutine(data));
        }
    }
    IEnumerator SpawnCoroutine(SpawnData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.interval);

            Vector3 spawnPosition = GetSpawnPosition();

            switch (data.type)
            {
                case SpawnType.Shark:
                    Factory.Instance.GetShark(spawnPosition);
                    break;
                
            }
        }
    }
    protected Vector3 GetSpawnPosition()
    {
        Vector3 result = transform.position;
        result.y = Random.Range(MinY, MaxY);
        return result;
    }
    Vector3 GetDestination()
    {
        Vector3 pos = destinationArea.position;
        pos.y += Random.Range(MinY, MaxY);

        return pos;
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //�������� �׸���
        if (destinationArea == null)
            destinationArea = transform.GetChild(0);
        //������� �׸���
        Gizmos.color = Color.yellow;
        Vector3 p0 = destinationArea.position + Vector3.up * MaxY;
        Vector3 p1 = destinationArea.position + Vector3.up * MinY;
        Gizmos.DrawLine(p0, p1);
    }

    void OnDrawGizmosSelected()
    {
        //�������� �׸���
        if (destinationArea == null)
            destinationArea = transform.GetChild(0);
        //������� �׸���
        Gizmos.color = Color.red;
        Vector3 p0 = destinationArea.position + MaxY * Vector3.up + 0.5f * Vector3.left;
        Vector3 p1 = destinationArea.position + MaxY * Vector3.up + 0.5f * Vector3.right;
        Vector3 p2 = destinationArea.position + MinY * Vector3.up + 0.5f * Vector3.right;
        Vector3 p3 = destinationArea.position + MinY * Vector3.up + 0.5f * Vector3.left;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
    }
#endif
}
