using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// TowerUpgradeScript keeps track of the upgrades what the changes are and what the upgrade level is.
/// </summary>
public class TowerUpgradeScript : MonoBehaviour
{
    public GameObject baseTowerModel;
    public List<int> upgradePathLevel = new List<int>() { 0, 0 };
    public List<int> upgradePathMaxLevel = new List<int>() { 2, 2 };
    public List<List<int>> upgradeCost = new List<List<int>>() {
                                                                new List<int>() { 100, 200 }, //upgrade path one cost
                                                                new List<int>() { 100, 200 }  //upgrade path two cost
                                                               };
    public List<List<string>> upgradeNames = new List<List<string>>() {
                                                                       new List<string>() { "Higher Fire Rate", "Bullet Rain" }, //upgrade path one names
                                                                       new List<string>() { "Depleted Plutonium", "Armor Piercing" }  //upgrade path two names
                                                                      };
    public List<List<GameObject>> upgradeIndicators = new List<List<GameObject>>() {
                                                                                    new List<GameObject>() { null, null },
                                                                                    new List<GameObject>() { null, null }
                                                                                   };
    public List<List<GameObject>> crosspathModels = new List<List<GameObject>>() {
                                                                                    new List<GameObject>() { null, null, null}, 
                                                                                    new List<GameObject>() { null, null, null},
                                                                                    new List<GameObject>() { null, null, null}
                                                                                 };
    [SerializeField] private GameObject modelZeroZero; [SerializeField] private GameObject modelZeroOne; [SerializeField] private GameObject modelZeroTwo;
    [SerializeField] private GameObject modelOneZero; [SerializeField] private GameObject modelOneOne; [SerializeField] private GameObject modelOneTwo;
    [SerializeField] private GameObject modelTwoZero; [SerializeField] private GameObject modelTwoOne; [SerializeField] private GameObject modelTwoTwo;

    public List<List<GameObject>> towerUpgrades = new List<List<GameObject>>() {
                                                                                  new List<GameObject>() { null, null, null },
                                                                                  new List<GameObject>() { null, null, null }
                                                                               };
    [SerializeField] private GameObject upgradePathOneOne; [SerializeField] private GameObject upgradePathOneTwo;
    [SerializeField] private GameObject upgradePathTwoOne; [SerializeField] private GameObject upgradePathTwoTwo;
    public GameObject currentActiveTowerModel;

    private void Start()
    {
        //declare crosspath models
        crosspathModels[0][0] = modelZeroZero;
        crosspathModels[0][1] = modelZeroOne;
        crosspathModels[0][2] = modelZeroTwo;
        crosspathModels[1][0] = modelOneZero;
        crosspathModels[1][1] = modelOneOne;
        crosspathModels[1][2] = modelOneTwo;
        crosspathModels[2][0] = modelTwoZero;
        crosspathModels[2][1] = modelTwoOne;
        crosspathModels[2][2] = modelTwoTwo;
        //declare upgrades
        towerUpgrades[0][1] = upgradePathOneOne;
        towerUpgrades[0][2] = upgradePathOneTwo;
        towerUpgrades[1][1] = upgradePathTwoOne;
        towerUpgrades[1][2] = upgradePathTwoTwo;
    }
    public void UpdateTower(int p)
    {
        GameObject newActiveTower = Instantiate(crosspathModels[upgradePathLevel[0]][upgradePathLevel[1]], currentActiveTowerModel.transform.position, Quaternion.identity, transform);
        Destroy(currentActiveTowerModel);
        currentActiveTowerModel = newActiveTower;
        Debug.Log("TowerUpgradeScript.UpdateTower: Upgrade Level Path One is: " + upgradePathLevel[0]);
        Debug.Log("TowerUpgradeScript.UpdateTower: Upgrade Level Path Two is: " + upgradePathLevel[1]);
        towerUpgrades[p][upgradePathLevel[p]].GetComponent<TowerUpgradeChanges>().upgradeTower(gameObject);
        GetComponent<TowerMenuScript>().towerRadius.transform.localScale = new Vector3(GetComponent<TowerStats>().radius, GetComponent<TowerMenuScript>().towerRadius.transform.localScale.y, GetComponent<TowerStats>().radius);

        //delete and update radius 
        //upgrade tower
        //play upgrade animation
    }
}
