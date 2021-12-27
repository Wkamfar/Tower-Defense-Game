using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public List<int> cost;
    [SerializeField] private float START_WAVE_TIMER;
    [SerializeField] private int waveCount;
    [SerializeField] private int baseMoney;
    public TextMeshProUGUI roundDisplay;
    private int currentWave;
    private float startWaveTimer;
    public GameObject enemy;
    private Queue<GameObject> enemiesToSpawn; // Queue<(GameObject, float)>
    private float enemySpawnTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        enemiesToSpawn = new Queue<GameObject>();
        for (int i = 0; i < 1; i++)
        {
            //enemiesToSpawn.Enqueue(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemySpawner.Update: The amount of AI: " + AIData.totalNumberOfAI);
        if (currentWave >= waveCount)
        {
            if (AIData.totalNumberOfAI == 0)
            {
                Debug.Log("You Win!");
            }
            return;
        }
        if (enemiesToSpawn.Count == 0)
        {
            startWaveTimer -= Time.deltaTime;
        }
        if (startWaveTimer <= 0)
        {
            StartNextWave();
        }
        if (MapData.isMapSpawned && enemiesToSpawn.Count > 0 && enemySpawnTimer <= 0)
        {
            GameObject unit = enemiesToSpawn.Dequeue();
            Spawn(unit);
            enemySpawnTimer = 1f;
        }
        else if (enemySpawnTimer > 0)
        {
            enemySpawnTimer -= Time.deltaTime;
        }
    }
    
    private void Spawn(GameObject _enemy)
    {
        GameObject currentEnemy = Instantiate(_enemy, MapData.PointToRealWorld(MapData.startingPoint), Quaternion.identity);
    }
    private void StartNextWave()
    {
        currentWave++;
        roundDisplay.text = currentWave.ToString() + " / " + waveCount.ToString();
        startWaveTimer = START_WAVE_TIMER;
        AIData.ChangeMoney(baseMoney * currentWave);
        bool canBuy = true;
        while (AIData.currentMoney != 0 && canBuy)
        {
            canBuy = false;
            for (int i = 0; i < enemyTypes.Count; ++i)
            {
                if (cost[i] <= AIData.currentMoney)
                {
                    canBuy = true;
                    AIData.ChangeMoney(-cost[i]);
                    enemiesToSpawn.Enqueue(enemyTypes[i]);
                }
            }
        }
    }
}
