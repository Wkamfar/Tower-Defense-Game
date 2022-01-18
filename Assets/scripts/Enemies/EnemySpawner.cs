using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour // Make preset waves to test out the enemy
{
    public List<GameObject> enemyTypes;
    [SerializeField] private float START_WAVE_TIMER;
    [SerializeField] private int waveCount;
    [SerializeField] private int baseMoney;
    public TextMeshProUGUI roundDisplay;
    private int currentWave;
    private float startWaveTimer;
    public GameObject enemy;
    public Queue<GameObject> enemiesToSpawn; // Queue<(GameObject, float, int)> // add spawn time from the start of the round // add what path they will go down
    private float enemySpawnTimer = 0;
    public bool win;
    public bool autostart;
    private bool haltStart;
    public bool startGame;
    public bool spawningWaveEnded;
    public bool waveEnded;
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
        //Debug to display all of the data
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < DataScript.waveLength.Count; ++i)
            {
                Debug.Log("EnemySpawner.Update: Wave " + (i + 1) + " Length is: " + DataScript.waveLength[i]);
            }
        }
        if (currentWave >= waveCount)
        {
            if (waveEnded)
            {
                win = true;
                return;
                //Debug.Log("You Win!");
            }
        }
        if (waveEnded && autostart && !haltStart)
        {
            StartNextWave();  
        }
        if (waveEnded && !autostart)
        {
            haltStart = true;
            startWaveTimer -= Time.deltaTime;
        }
        if (startWaveTimer <= 0 && startGame)
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
        if (enemiesToSpawn.Count == 0 && !spawningWaveEnded && startGame)
        {
            spawningWaveEnded = true;
            DataScript.currentSpawningWaveEnd = Time.time;
            DataScript.spawningWaveLength.Add(DataScript.currentSpawningWaveEnd - DataScript.currentWaveStart);
        }
        if (enemiesToSpawn.Count == 0 && AIData.enemies.Count == 0 && !waveEnded && startGame)
        {
            waveEnded = true;
            DataScript.currentWaveEnd = Time.time;
            DataScript.waveLength.Add(DataScript.currentWaveEnd - DataScript.currentWaveStart);
        }
    }
    
    private void Spawn(GameObject _enemy)
    {
        GameObject currentEnemy = Instantiate(_enemy, MapData.PointToRealWorld(MapData.startingPoint), Quaternion.identity);
    }
    public void StartNextWave()
    {
        spawningWaveEnded = false;
        waveEnded = false;
        haltStart = false;
        currentWave++;
        AIData.currentWave = currentWave;
        CreateDataRatings.SetTotalPotentialDPS(true);
        CreateDataRatings.SetTotalPotentialDPS(false);
        DataScript.currentWaveStart = Time.time;
        roundDisplay.text = currentWave.ToString() + " / " + waveCount.ToString();
        startWaveTimer = START_WAVE_TIMER;
        AIData.ChangeMoney(baseMoney * currentWave);
        bool canBuy = true;
        while (AIData.currentMoney != 0 && canBuy)
        {
            canBuy = false;
            for (int i = 0; i < enemyTypes.Count; ++i)
            {
                if (enemyTypes[i].GetComponent<EnemyAI>().cost <= AIData.currentMoney)
                {
                    canBuy = true;
                    AIData.ChangeMoney(-enemyTypes[i].GetComponent<EnemyAI>().cost);
                    enemiesToSpawn.Enqueue(enemyTypes[i]);
                }
            }
        }
    }
}
