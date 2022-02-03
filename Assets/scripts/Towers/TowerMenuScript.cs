using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerMenuScript : MonoBehaviour
{
    //Add a close button later
    public GameObject towerMenu;
    public GameObject towerRadius;
    public GameObject targetDisplay;
    private int _targetingIndex = 0;
    public GameObject upgradeOneDisplay;
    public GameObject upgradeTwoDisplay;
    public GameObject damageDealtDisplay;
    public GameObject xButton;
    // Start is called before the first frame update
    void Start()
    {
        OpenTowerMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTowerMenu();
        }
    }
    public void OpenTowerMenu()
    {
        GameObject[] towerMenus = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject t in towerMenus)
        {
            t.GetComponent<TowerMenuScript>().CloseTowerMenu();
        }
        towerMenu.SetActive(true);
        towerRadius.GetComponent<Renderer>().enabled = true;
    }
    public void CloseTowerMenu()
    {
            if (GetComponent<TowerTargeting>().choosingMarker)
            {
                GetComponent<TowerTargeting>().CancelNewMarker();
            }
            GetComponent<TowerSpecialItemScript>().DeactivateSpecialItemPlacement(false);
            towerMenu.SetActive(false);
            towerRadius.GetComponent<Renderer>().enabled = false;     
    }
    public void Sell()
    {
        GetComponent<TowerActionScript>().OnDestroyTower();
        PlayerData.ChangeMoney(GetComponent<TowerStats>().SellValue());
        TowerData.towers.Remove(gameObject);
        Destroy(towerMenu);
        Destroy(towerRadius);
        Destroy(gameObject);
    }
    public void UpdateUpgradePath(int p, TextMeshProUGUI upgradeNameDisplay, TextMeshProUGUI upgradeCostDisplay)
    {
        if (GetComponent<TowerUpgradeScript>().upgradePathMaxLevel[p] > GetComponent<TowerUpgradeScript>().upgradePathLevel[p] &&
            GetComponent<TowerUpgradeScript>().upgradeCost[p][GetComponent<TowerUpgradeScript>().upgradePathLevel[p]] <= PlayerData.playerMoney)
        {
            PlayerData.ChangeMoney(-GetComponent<TowerUpgradeScript>().upgradeCost[p][GetComponent<TowerUpgradeScript>().upgradePathLevel[p]]);
            GetComponent<TowerStats>().AddValue(GetComponent<TowerUpgradeScript>().upgradeCost[p][GetComponent<TowerUpgradeScript>().upgradePathLevel[p]]);
            GetComponent<TowerUpgradeScript>().upgradeIndicators[p][GetComponent<TowerUpgradeScript>().upgradePathLevel[p]].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
            ++GetComponent<TowerUpgradeScript>().upgradePathLevel[p];
            GetComponent<TowerUpgradeScript>().UpdateTower(p);
            if (GetComponent<TowerUpgradeScript>().upgradePathMaxLevel[p] > GetComponent<TowerUpgradeScript>().upgradePathLevel[p])
            {
                upgradeNameDisplay.text = GetComponent<TowerUpgradeScript>().upgradeNames[p][GetComponent<TowerUpgradeScript>().upgradePathLevel[p]];
                upgradeCostDisplay.text = GetComponent<TowerUpgradeScript>().upgradeCost[p][GetComponent<TowerUpgradeScript>().upgradePathLevel[p]].ToString();
            }
            else
            {
                //Change this later to make it neater
                upgradeNameDisplay.text = "Max Level";
                upgradeCostDisplay.text = "";
            }
        }
    }
    public void ChangeTargetLeft()
    {
        _targetingIndex = _targetingIndex == 0 ? GetComponent<TowerStats>().targetingOptions.Count - 1 : _targetingIndex - 1;
        UpdateTarget();
    }
    public void ChangeTargetRight()
    {
        _targetingIndex = (_targetingIndex + 1) % GetComponent<TowerStats>().targetingOptions.Count;
        UpdateTarget();
    }
    private void UpdateTarget()
    {
        GetComponent<TowerStats>().targetingIndex = _targetingIndex;
        targetDisplay.GetComponent<TextMeshProUGUI>().text = GetComponent<TowerStats>().targetingOptionNames[GetComponent<TowerStats>().targetingOptions[_targetingIndex]];
    }
}
