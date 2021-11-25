using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class PlayerActionScript : MonoBehaviour
{
    public float height;
    public GameObject towerMenu;
    public GameObject towerMenuHolder;
    public GameObject towerRadius;
    public GameObject mouseFollower;
    public GameObject towerModelDisplay;
    public GameObject towerDisplayRadius;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPos = new Vector3(mousePos.x, height, mousePos.z);
        if (TowerData.hasSelectedTower && !IsMouseOverUI())
        {
            mouseFollower.SetActive(true);
            mouseFollower.transform.position = spawnPos;
            towerDisplayRadius.transform.localScale = new Vector3(TowerData.selectedTower.GetComponent<TowerStats>().radius, towerDisplayRadius.transform.localScale.y, TowerData.selectedTower.GetComponent<TowerStats>().radius);
            float hitboxSize = 2 * TowerData.selectedTower.GetComponent<TowerStats>().hitbox;
            towerModelDisplay.transform.localScale = new Vector3(hitboxSize, hitboxSize, hitboxSize);
            if (!CanPlaceTower())
            {
                towerDisplayRadius.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.3f);
            }
            else
            {
                towerDisplayRadius.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.3f);
            }
        }
        else
        {
            mouseFollower.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanPlaceTower())
            {
                SpawnTower(spawnPos);
            }
            //fix this in class
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask layerMask = LayerMask.GetMask("Tower");
            if (CanSelectTower() && Physics.Raycast(ray, out hit, 100, layerMask))
            {
                hit.collider.gameObject.GetComponent<TowerMenuScript>().OpenTowerMenu();
            }
            else if (CanSelectTower())
            {
                GameObject[] towerMenus = GameObject.FindGameObjectsWithTag("Tower");
                foreach (GameObject t in towerMenus)
                {
                    t.GetComponent<TowerMenuScript>().CloseTowerMenu();
                }
            }
        }
    }
    private bool CanPlaceTower()
    {
        if (!TowerData.hasSelectedTower || IsMouseOverUI() || PlayerData.playerMoney < TowerData.selectedTower.GetComponent<TowerStats>().cost || TooCloseToOtherTower() || WrongBlock() || !SpecialRequirement())
        {
            return false;
        }
        return true;
    }
    private bool TooCloseToOtherTower()
    {
        foreach (GameObject t in TowerData.towers)
        {
            float x = Mathf.Abs(mouseFollower.transform.position.x - t.transform.position.x);
            float y = Mathf.Abs(mouseFollower.transform.position.z - t.transform.position.z);
            float distance = Mathf.Sqrt(x * x + y * y);
            if (distance < TowerData.selectedTower.GetComponent<TowerStats>().hitbox + t.GetComponent<TowerStats>().hitbox)
            {
                return true;
            }
        }
        return false;
    }
    private bool WrongBlock()
    {
        List<GameObject> allowedBlocks = TowerData.selectedTower.GetComponent<TowerStats>().allowedBlocks;
        
        return false;
    }
    private bool SpecialRequirement()
    {
        return true;
    }

    private bool CanSelectTower()
    {
        if (TowerData.hasSelectedTower || IsMouseOverUI())
        {
            return false;
        }
        return true;
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void SpawnTower(Vector3 spawnPos)
    {
        //Add a spawning effect
        GameObject currentTower = Instantiate(TowerData.selectedTower, spawnPos, Quaternion.identity);
        PlayerData.ChangeMoney(-currentTower.GetComponent<TowerStats>().cost);
        SpawnRadius(currentTower);
        SpawnMenu(currentTower);
        TowerData.towers.Add(currentTower);
    }
    private void SpawnRadius(GameObject tower)
    {
        GameObject currentRadius = Instantiate(towerRadius, tower.transform);
        tower.GetComponent<TowerMenuScript>().towerRadius = currentRadius;
        currentRadius.transform.localScale = new Vector3(tower.GetComponent<TowerStats>().radius, currentRadius.transform.localScale.y, tower.GetComponent<TowerStats>().radius);
        currentRadius.GetComponent<RadiusDetection>().tower = tower;
    }
    private void SpawnMenu(GameObject tower)
    {
        GameObject currentTowerMenu = Instantiate(towerMenu, towerMenuHolder.transform);
        if (tower.transform.position.x < MapData.mapCenter.x)
        {
            currentTowerMenu.transform.localPosition = new Vector3(390, 0, 0);
        }
        else
        {
            currentTowerMenu.transform.localPosition = new Vector3(-740, 0, 0);
        }
        tower.GetComponent<TowerMenuScript>().towerMenu = currentTowerMenu;
        GameObject towerName = currentTowerMenu.transform.GetChild(0).gameObject;
        towerName.GetComponent<TextMeshProUGUI>().text = TowerData.selectedTower.name;
    }
}
