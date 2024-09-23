using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    protected const float MaxX = 8.0f;
    protected const float MinX = -8.0f;
    public float interval =2.0f;
    GameOverTime gameOverTime;
    private void OnDrawGizmos()
    {
        // 출발지점 그리기
        Gizmos.color = Color.green;
        Vector3 p0 = transform.position + Vector3.right * MaxX;
        Vector3 p1 = transform.position + Vector3.right * MinX;
        Gizmos.DrawLine(p0, p1);
    }
    private void Awake()
    {
        gameOverTime = GameManager.Instance.GameOverTime;
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
            // Debug.Log($"연속스폰 시작 : {Time.time}");
            Vector3 spawnPosition = GetSpawnPosition();

            Factory.Instance.GetSpike(spawnPosition);

            yield return new WaitForSeconds(interval/gameOverTime.level);
        }
    }
    protected Vector3 GetSpawnPosition()
    {
        Vector3 result = transform.position;
        result.x = Random.Range(MinX, MaxX);
        return result;
    }
}
