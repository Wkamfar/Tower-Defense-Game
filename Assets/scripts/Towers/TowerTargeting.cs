using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// adds anything that comes into the radius and can be shot into a list, and then from there choose the priority, either strongest target, weakest target, first target, or last target, and then remove them when they leave the radius
/// Is used in TowerActionScirpt
/// </summary>
public class TowerTargeting : MonoBehaviour
{

    public bool hasMarker;
    public GameObject changeMarkerButton;
    public GameObject marker;
    public GameObject currentMarker;
    public Canvas markerCanvas;
    public bool choosingMarker;
    public GameObject mouseFollower;
    private List<GameObject> targets = new List<GameObject>();
    private void Update()
    {
        targets = GetComponent<TowerStats>().targets;
        if (choosingMarker)
        {
            mouseFollower.transform.position = Input.mousePosition;
        }
        if (choosingMarker && Input.GetKeyDown(KeyCode.Mouse0) && IsInMap())
        {
            PlaceChooseMarker();
        }
    }
    public Vector3 targeting(int mode)
    {
        hasMarker = false;
        changeMarkerButton.SetActive(false);
        if (currentMarker != null) { currentMarker.SetActive(false); }
        if (mode == 0)
        {
            return FirstEnemy();
        }
        else if (mode == 1)
        {
            return LastEnemy();
        }
        else if (mode == 2)
        {
            return StrongestEnemy();
        }
        else if (mode == 3)
        {
            return WeakestEnemy();
        }
        else if (mode == 4)
        {
            return TargetMarker();
        }
        else if (mode == 5)
        {
            return FollowMouse();
        }
        //Add more firing modes later, and maybe some specialty ones
        return new Vector3();
    }
    public Vector3 StrongestEnemy()
    {
        if (targets.Count > 0)
        {

        }
        return new Vector3();
    }
    public Vector3 WeakestEnemy()
    {
        if (targets.Count > 0)
        {

        }
        return new Vector3();
    }//Do these two later
    public Vector3 FirstEnemy()
    {
        while (targets.Count > 0 && (!targets[0] || targets[0].GetComponent<EnemyAI>().IsDead))
        {
            DeleteDeadObject(0);
        }
        if (targets.Count > 0)
        {
            return targets[0].transform.position;
        }
        return new Vector3();
    }
    public Vector3 LastEnemy()
    {
        if (targets.Count > 0)
        {
            return targets[targets.Count - 1].transform.position;
        }
        return new Vector3();
    }
    public Vector3 TargetMarker() 
    {
        changeMarkerButton.SetActive(true);
        hasMarker = true;
        if (currentMarker == null)
        {
            Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position); //make it smarter so that it aims at path or enemy when placed or something like that
            currentMarker = Instantiate(marker, spawnPos, Quaternion.identity, markerCanvas.transform);
        }
        if (GetComponent<TowerMenuScript>().towerMenu.activeInHierarchy || choosingMarker)
        {
            currentMarker.SetActive(true);
        }        
        Vector3 markerPos = Camera.main.ScreenToWorldPoint(currentMarker.transform.position);
        return new Vector3(markerPos.x, transform.position.y, markerPos.z);
    }
    public void ChooseNewMarker()
    {
        choosingMarker = true;
        GetComponent<TowerMenuScript>().towerMenu.SetActive(false);
        mouseFollower = Instantiate(marker, markerCanvas.transform);
    }
    public void PlaceChooseMarker()
    {
        choosingMarker = false;
        GetComponent<TowerMenuScript>().towerMenu.SetActive(true);
        currentMarker.transform.position = Input.mousePosition;
        Destroy(mouseFollower);
    }
    public Vector3 FollowMouse() 
    {
        hasMarker = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, transform.position.y, mousePos.z);
    }
    private void DeleteDeadObject(int i)
    {
        targets.Remove(targets[i]);
    }
    private bool IsInMap()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xPoint = (mousePos.x - MapData.offset[0]) / MapData.offset[2];
        float yPoint = (mousePos.z - MapData.offset[1]) / -MapData.offset[2];
        //Debug.Log("PlayerActionScript.IsInMap: The xPoint is: " + xPoint + ", and the yPoint is: " + yPoint);
        if (yPoint < MapData.grid.Count - 1 && yPoint >= 0 && xPoint < MapData.grid[0].Count - 1 && xPoint >= 0)
        {
            return true;
        }
        return false;
    }
}
