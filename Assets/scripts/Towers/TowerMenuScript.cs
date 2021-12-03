using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerMenuScript : MonoBehaviour
{
    //Add a close button later
    public GameObject towerMenu;
    public GameObject towerRadius;
    public GameObject targetDisplay;
    private int _targetingIndex = 0;
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
    public void UpgradePathOne()
    {
        
    }
    public void UpgradePathTwo()
    {
        
    }
    public void ChangeTargetLeft()
    {
        _targetingIndex = _targetingIndex == 0 ? this.GetComponent<TowerStats>().targetingOptions.Count - 1 : _targetingIndex - 1;
        this.GetComponent<TowerStats>().targetingIndex = _targetingIndex;
        targetDisplay.GetComponent<TextMeshProUGUI>().text = this.GetComponent<TowerTargeting>().targetingOptionNames[this.GetComponent<TowerStats>().targetingOptions[_targetingIndex]];
    }
    public void ChangeTargetRight()
    {
        _targetingIndex = (_targetingIndex + 1) % this.GetComponent<TowerStats>().targetingOptions.Count;
        this.GetComponent<TowerStats>().targetingIndex = _targetingIndex;
        targetDisplay.GetComponent<TextMeshProUGUI>().text = this.GetComponent<TowerTargeting>().targetingOptionNames[this.GetComponent<TowerStats>().targetingOptions[_targetingIndex]];
    }
}
