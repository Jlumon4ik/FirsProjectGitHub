using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour
{
    [SerializeField] private Transform[] spawPoint;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timetoSpawn;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void Spawn()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = spawPoint[Random.Range(0, spawPoint.Length)].position;
        Destroy(enemy, 30);
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(timetoSpawn);
            Spawn();
        }

    }
}

