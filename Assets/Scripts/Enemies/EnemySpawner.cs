using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;//список групп врагов для спавна в волну
        public int waveQuota; //количество врагов на волну
        public int spawnInterval;//интервал с которым они спавнятся
        public int spawnCount;//кол-во врагов уже отспавненых

    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;//кол-во врагов необходимых заспавнить в волну
        public int spawnCount;//кол-во врагов этого типа уже заспавненных
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; //Список всех волн
    public int currentWaveCount; //индекс текущей волны (начинается с 0)

    [Header("Spawner Attributes")]
    float spawnTimer; //таймер для спавна следующего врага
    public int enemiesAlive;
    public int maxEnemiesAllowed; //максимальное количество врагов на карту
    public bool maxEnemiesReached = false;
    public float waveInterval;//интервал между волнами
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints;

    Transform player;

    void Start()
    { 
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)  //проверка, закончилась ли волна
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;

        yield return new WaitForSeconds(waveInterval);

        //проверка на количество волн которые ещё можно спавнить после текущей
        if(currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }
    void SpawnEnemies()
    {
        //чек если уже отспавнился минимум врагов для волны
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //спавн каждого врага до выполнения квоты
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //чек если минимальное кол-во врагов этого типа уже отспавнено
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}
