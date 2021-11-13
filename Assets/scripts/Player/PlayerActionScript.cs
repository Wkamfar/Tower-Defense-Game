using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject mouseFollower;
    public GameObject towerRadius;
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 spawnPos = new Vector3(mousePos.x, height, mousePos.z);
        mouseFollower.transform.position = spawnPos;
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanPlaceTower())
        {
            Spawn(spawnPos);
        }
    }
    private bool CanPlaceTower()
    {
        if (!TowerData.hasSelectedTower)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void Spawn(Vector3 spawnPos)
    {
        GameObject currentTower = Instantiate(TowerData.selectedTower, spawnPos, Quaternion.identity);
        //Add a spawning effect
        GameObject currentRadius = Instantiate(towerRadius, currentTower.transform);
        currentRadius.transform.localScale = new Vector3(currentTower.GetComponent<TowerStats>().Radius, currentRadius.transform.localScale.y, currentTower.GetComponent<TowerStats>().Radius);
        currentRadius.GetComponent<RadiusDetection>().tower = currentTower;
    }
}
