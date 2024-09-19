using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    Transform spawnPoint1;
    Transform spawnPoint2;
    protected const float MaxY = 2.0f;
    protected const float MinY = -2.0f;
    public float interval = 2.0f;


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        if(spawnPoint1 == null)
        {
            spawnPoint1= transform.GetChild(0);
        }
        Vector3 p0 = spawnPoint1.position + Vector3.up * MaxY;
        Vector3 p1 = spawnPoint1.position + Vector3.up * MinY;
        Gizmos.DrawLine(p0, p1);
        if(spawnPoint2 == null)
        {
            spawnPoint2= transform.GetChild(1);
        }
        p0 = spawnPoint1.position + Vector3.up * MaxY;
        p1 = spawnPoint1.position + Vector3.up * MinY;
        Gizmos.DrawLine(p0, p1);
    }
    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }
    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(interval);


        while (true)
        {
            // Debug.Log($"���ӽ��� ���� : {Time.time}");
            Vector3 spawnPosition = GetSpawnPosition();

            Factory.Instance.GetSpike(spawnPosition);

            yield return new WaitForSeconds(interval);
        }
    }
    protected Vector3 GetSpawnPosition()
    {
        Vector3 result = transform.position;
        result.y = Random.Range(MinY, MaxY);
        return result;
    }

}