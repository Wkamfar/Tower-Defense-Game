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
        towerMenu.SetActive(false);
        towerRadius.GetComponent<Renderer>().enabled = false;
    }
    public void Sell()
    {
        PlayerData.ChangeMoney(this.gameObject.GetComponent<TowerStats>().SellValue());
        TowerData.towers.Remove(this.gameObject);
        Destroy(towerMenu);
        Destroy(towerRadius);
        Destroy(this.gameObject);
    }
    public void UpdateUpgradePath(int p, TextMeshProUGUI upgradeNameDisplay, TextMeshProUGUI upgradeCostDisplay)
    {
        if (this.GetComponent<TowerUpgradeScript>().upgradePathMaxLevel[p] > this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p] &&
            this.GetComponent<TowerUpgradeScript>().upgradeCost[p][this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p]] <= PlayerData.playerMoney)
        {
            PlayerData.ChangeMoney(-this.GetComponent<TowerUpgradeScript>().upgradeCost[p][this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p]]);
            this.GetComponent<TowerStats>().AddValue(this.GetComponent<TowerUpgradeScript>().upgradeCost[p][this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p]]);
            this.gameObject.GetComponent<TowerUpgradeScript>().upgradeIndicators[p][this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p]].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
            ++this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p];
            GetComponent<TowerUpgradeScript>().UpdateTower(p);
            if (this.GetComponent<TowerUpgradeScript>().upgradePathMaxLevel[p] > this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p])
            {
                upgradeNameDisplay.text = this.gameObject.GetComponent<TowerUpgradeScript>().upgradeNames[p][this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p]];
                upgradeCostDisplay.text = this.gameObject.GetComponent<TowerUpgradeScript>().upgradeCost[p][this.GetComponent<TowerUpgradeScript>().upgradePathLevel[p]].ToString();
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
        _targetingIndex = _targetingIndex == 0 ? this.GetComponent<TowerStats>().targetingOptions.Count - 1 : _targetingIndex - 1;
        UpdateTarget();
    }
    public void ChangeTargetRight()
    {
        _targetingIndex = (_targetingIndex + 1) % this.GetComponent<TowerStats>().targetingOptions.Count;
        UpdateTarget();
    }
    private void UpdateTarget()
    {
        this.GetComponent<TowerStats>().targetingIndex = _targetingIndex;
        targetDisplay.GetComponent<TextMeshProUGUI>().text = this.GetComponent<TowerStats>().targetingOptionNames[this.GetComponent<TowerStats>().targetingOptions[_targetingIndex]];
    }
}
