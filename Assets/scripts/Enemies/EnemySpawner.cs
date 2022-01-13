using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    [SerializeField] private float START_WAVE_TIMER;
    [SerializeField] private int waveCount;
    [SerializeField] private int baseMoney;
    public TextMeshProUGUI roundDisplay;
    private int currentWave;
    private float startWaveTimer;
    public GameObject enemy;
    public Queue<GameObject> enemiesToSpawn; // Queue<(GameObject, float)>
    private float enemySpawnTimer = 0;
    public bool win;
    public bool autostart;
    private bool haltStart;
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
            if (AIData.enemies.Count == 0)
            {
                win = true;
                //Debug.Log("You Win!");
            }
            return;
        }
        if (enemiesToSpawn.Count == 0 && AIData.enemies.Count == 0 && autostart && !haltStart)
        {
            StartNextWave();  
        }
        if (enemiesToSpawn.Count == 0 && AIData.enemies.Count == 0 && !autostart)
        {
            haltStart = true;
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
    public void StartNextWave()
    {
        haltStart = false;
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
