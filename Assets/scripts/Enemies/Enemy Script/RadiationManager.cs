using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationManager : MonoBehaviour
{
    private int maxRadCount = 100;
    public int radCount = 0;
    private float radBaseDamage = 0.2f;
    private float radDecayRate = 120;
    private float radDecayTimer;
    private List<GameObject> towers = new List<GameObject>();
    private List<int> towerRadCounts = new List<int>();
    void Update()
    {
        if (radCount > 0)
        {
            if (radDecayTimer <= 0)
            {
                //Debug.Log("RadiationManager.Update: The size of towerRadCounts is: " + towerRadCounts.Count);
                if (towerRadCounts.Count > 0)
                {
                    if (towerRadCounts[towerRadCounts.Count - 1] > 0)
                    {
                        GetComponent<EnemyAI>().TakeDamage(radBaseDamage + radCount / 1000, towers[towers.Count - 1]);
                        --towerRadCounts[towerRadCounts.Count - 1];
                        --radCount;
                    }
                    else
                    {
                        towers.RemoveAt(towers.Count - 1);
                        towerRadCounts.RemoveAt(towerRadCounts.Count - 1);
                    }
                }
            }
            radDecayTimer = radDecayTimer > 0 ? radDecayTimer -= Time.deltaTime : 60 / (radDecayRate + radDecayRate * (radCount / maxRadCount));
        }
    }
    public void IncreaseRadCount(GameObject tower)
    {
        if (radCount + tower.GetComponent<LaserShoot>().radCount < maxRadCount)
        {
            //Debug.Log("RadiationManager.IncreaseRadCount: radCount + tower.GetComponent<LaserShoot>().radCount < 100");
            towers.Add(tower);
            towerRadCounts.Add(tower.GetComponent<LaserShoot>().radCount);
        }
        else if (radCount < 100 && radCount + tower.GetComponent<LaserShoot>().radCount >= maxRadCount)
        {
            //Debug.Log("RadiationManager.IncreaseRadCount: radCount < 100 && radCount + tower.GetComponent<LaserShoot>().radCount >= 100");
            int netRad = maxRadCount - (radCount + tower.GetComponent<LaserShoot>().radCount);
            towers.Add(tower);
            towerRadCounts.Add(netRad);
        }
        radDecayTimer = 60 / radDecayRate;
        radCount = radCount < maxRadCount ? radCount + tower.GetComponent<LaserShoot>().radCount : maxRadCount;
        //Debug.Log("RadiationManager.IncreaseRadCount: Rad Count has been increased: " + radCount);
    }
}
