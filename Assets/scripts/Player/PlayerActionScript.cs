using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class PlayerActionScript : MonoBehaviour
{
    /*public Ray mousePos;
    public float spawningHeight;
    RaycastHit hit;
    public GameObject tower;
    void Update()
    {
        mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(mousePos, out hit))
            {
                Instantiate(tower, new Vector3(hit.transform.position.x, spawningHeight, hit.transform.position.z), Quaternion.identity);
            }
                
        }
        
        
        //Debug.Log("The mouse position is: (" + mousePos.x + ", " + mousePos.z + ")");
    }*/
    public float height;
    public GameObject towerMenu;
    public GameObject towerMenuHolder;
    public GameObject towerRadius;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPos = new Vector3(mousePos.x, height, mousePos.z);
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
            if (!CanPlaceTower() && Physics.Raycast(ray, out hit, 100, layerMask))
            {
                hit.collider.gameObject.GetComponent<TowerMenuScript>().OpenTowerMenu();
            }
            else if (CanPlaceTower())
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
        if (!TowerData.hasSelectedTower || IsMouseOverUI() || PlayerData.playerMoney < TowerData.selectedTower.GetComponent<TowerStats>().cost)
        {
            return false;
        }
        else
        {
            return true;
        }
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
    }
    private void SpawnRadius(GameObject tower)
    {
        GameObject currentRadius = Instantiate(towerRadius, tower.transform);
        tower.GetComponent<TowerMenuScript>().towerRadius = currentRadius;
        currentRadius.transform.localScale = new Vector3(tower.GetComponent<TowerStats>().Radius, currentRadius.transform.localScale.y, tower.GetComponent<TowerStats>().Radius);
        currentRadius.GetComponent<RadiusDetection>().tower = tower;
    }
    private void SpawnMenu(GameObject tower)
    {
        GameObject currentTowerMenu = Instantiate(towerMenu, towerMenuHolder.transform);
        tower.GetComponent<TowerMenuScript>().towerMenu = currentTowerMenu;
        GameObject towerName = currentTowerMenu.transform.GetChild(0).gameObject;
        towerName.GetComponent<TextMeshProUGUI>().text = TowerData.selectedTower.name;
    }
}
