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
    public GameObject currentActiveTowerModel;
}
