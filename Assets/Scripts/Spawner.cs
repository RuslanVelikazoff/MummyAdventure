using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Treasure Settings")]
    public int treasureCount;
    [SerializeField] private List<Transform> treasurePoints;
    [SerializeField] private GameObject treasurePrefab;
    [Header("Enemy Settings")]
    public int enemyCount;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    private void Start()
    {
        for (int i = 0; i < treasureCount; i++)
        {
            Instantiate(treasurePrefab, treasurePoints[Random.Range(0, treasurePoints.Count)]);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            int rand = Random.Range(0, enemySpawnPoints.Count);
            if(enemySpawnPoints[rand].childCount == 0)
            {
                GameObject go = Instantiate(enemyPrefab, enemySpawnPoints[rand]);
                go.GetComponent<Enemy>().randomDestination = treasurePoints;
            }
            else
            {
                rand = Random.Range(0, enemySpawnPoints.Count);
                GameObject go = Instantiate(enemyPrefab, enemySpawnPoints[rand]);
                go.GetComponent<Enemy>().randomDestination = treasurePoints;
            }
        }
    }
}
