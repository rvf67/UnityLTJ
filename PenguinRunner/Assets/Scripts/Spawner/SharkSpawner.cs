using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    Transform[] spawnPoints;
    public float interval=3.0f;
    private void Awake()
    {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i]= transform.GetChild(i).transform;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnShark());
    }

    IEnumerator SpawnShark()
    {
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        while (true)
        {
            Factory.Instance.GetShark(spawnPoints[randomSpawnPoint].position);
            yield return new WaitForSeconds(interval);
            randomSpawnPoint = Random.Range(0,spawnPoints.Length);
        }
    }
}
