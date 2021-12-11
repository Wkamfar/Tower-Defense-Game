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
    //Make a bar that fills up squares until you reach max level
    public void UpgradePathOne()
    {
        if (this.GetComponent<TowerStats>().upgradePathMaxLevel[0] > this.GetComponent<TowerStats>().upgradePathLevel[0] &&
            this.GetComponent<TowerStats>().upgradeCost[0][this.GetComponent<TowerStats>().upgradePathLevel[0]] <= PlayerData.playerMoney)
        {
            PlayerData.ChangeMoney(-this.GetComponent<TowerStats>().upgradeCost[0][this.GetComponent<TowerStats>().upgradePathLevel[0]]);
            this.GetComponent<TowerStats>().AddValue(this.GetComponent<TowerStats>().upgradeCost[1][this.GetComponent<TowerStats>().upgradePathLevel[0]]);
            TextMeshProUGUI upgradeNameDisplay = upgradeOneDisplay.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI upgradeCostDisplay = upgradeOneDisplay.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            this.gameObject.GetComponent<TowerStats>().upgradeIndicators[0][this.GetComponent<TowerStats>().upgradePathLevel[0]].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
            ++this.GetComponent<TowerStats>().upgradePathLevel[0];
            if (this.GetComponent<TowerStats>().upgradePathMaxLevel[0] > this.GetComponent<TowerStats>().upgradePathLevel[0])
            {
                upgradeNameDisplay.text = this.gameObject.GetComponent<TowerStats>().upgradeNames[0][this.GetComponent<TowerStats>().upgradePathLevel[0]];
                upgradeCostDisplay.text = this.gameObject.GetComponent<TowerStats>().upgradeCost[0][this.GetComponent<TowerStats>().upgradePathLevel[0]].ToString();
            }
            else
            {
                //Change this later to make it neater
                upgradeNameDisplay.text = "Max Level"; 
                upgradeCostDisplay.text = "";
            }
        }
    }
    public void UpgradePathTwo()
    {
        if (this.GetComponent<TowerStats>().upgradePathMaxLevel[1] > this.GetComponent<TowerStats>().upgradePathLevel[1] &&
            this.GetComponent<TowerStats>().upgradeCost[1][this.GetComponent<TowerStats>().upgradePathLevel[1]] <= PlayerData.playerMoney)
        {
            PlayerData.ChangeMoney(-this.GetComponent<TowerStats>().upgradeCost[1][this.GetComponent<TowerStats>().upgradePathLevel[1]]);
            this.GetComponent<TowerStats>().AddValue(this.GetComponent<TowerStats>().upgradeCost[1][this.GetComponent<TowerStats>().upgradePathLevel[1]]);
            TextMeshProUGUI upgradeNameDisplay = upgradeTwoDisplay.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI upgradeCostDisplay = upgradeTwoDisplay.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            this.gameObject.GetComponent<TowerStats>().upgradeIndicators[1][this.GetComponent<TowerStats>().upgradePathLevel[1]].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
            ++this.GetComponent<TowerStats>().upgradePathLevel[1];
            if (this.GetComponent<TowerStats>().upgradePathMaxLevel[1] > this.GetComponent<TowerStats>().upgradePathLevel[1])
            {
                upgradeNameDisplay.text = this.gameObject.GetComponent<TowerStats>().upgradeNames[1][this.GetComponent<TowerStats>().upgradePathLevel[1]];
                upgradeCostDisplay.text = this.gameObject.GetComponent<TowerStats>().upgradeCost[1][this.GetComponent<TowerStats>().upgradePathLevel[1]].ToString();
            }
            else
            {
                //Change this later to make it neater
                upgradeNameDisplay.text = "Max Level";
                upgradeCostDisplay.text = "";
            }
        }
    }
    private void UpdateUpgradePath(int p)
    {
        PlayerData.ChangeMoney(-this.GetComponent<TowerStats>().upgradeCost[p][this.GetComponent<TowerStats>().upgradePathLevel[p]]);
        this.GetComponent<TowerStats>().AddValue(this.GetComponent<TowerStats>().upgradeCost[p][this.GetComponent<TowerStats>().upgradePathLevel[p]]);
        TextMeshProUGUI upgradeNameDisplay = upgradeOneDisplay.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI upgradeCostDisplay = upgradeOneDisplay.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        this.gameObject.GetComponent<TowerStats>().upgradeIndicators[p][this.GetComponent<TowerStats>().upgradePathLevel[p]].GetComponent<RawImage>().color = new Color(0f, 1f, 0f, 0.3f);
        ++this.GetComponent<TowerStats>().upgradePathLevel[p];
        if (this.GetComponent<TowerStats>().upgradePathMaxLevel[p] > this.GetComponent<TowerStats>().upgradePathLevel[p])
        {
            upgradeNameDisplay.text = this.gameObject.GetComponent<TowerStats>().upgradeNames[p][this.GetComponent<TowerStats>().upgradePathLevel[p]];
            upgradeCostDisplay.text = this.gameObject.GetComponent<TowerStats>().upgradeCost[p][this.GetComponent<TowerStats>().upgradePathLevel[p]].ToString();
        }
        else
        {
            //Change this later to make it neater
            upgradeNameDisplay.text = "Max Level";
            upgradeCostDisplay.text = "";
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
        targetDisplay.GetComponent<TextMeshProUGUI>().text = this.GetComponent<TowerTargeting>().targetingOptionNames[this.GetComponent<TowerStats>().targetingOptions[_targetingIndex]];
    }
}
