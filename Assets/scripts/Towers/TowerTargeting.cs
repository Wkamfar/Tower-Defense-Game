using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// adds anything that comes into the radius and can be shot into a list, and then from there choose the priority, either strongest target, weakest target, first target, or last target, and then remove them when they leave the radius
/// Is used in TowerActionScirpt
/// </summary>
public class TowerTargeting : MonoBehaviour
{
    public float smoothnessFactor = 0.1f;
    public bool hasMarker;
    public GameObject changeMarkerButton;
    public GameObject marker;
    public GameObject currentMarker;
    public Canvas markerCanvas;
    public bool choosingMarker;
    public GameObject mouseFollower;
    private List<GameObject> targets = new List<GameObject>();
    public Canvas gameUI;
    private void Update()
    {
        targets = GetComponent<TowerStats>().targets;
        for (int i = 0; i < targets.Count; ++i)
        {
            bool isAlive = false;
            for (int j = 0;  j < AIData.enemies.Count; ++j)
            {
                if (targets[i] == AIData.enemies[j])
                {
                    isAlive = true;
                    break;
                }
            }
            if (!isAlive)
            {
                DeleteDeadObject(i);
            }
        }
        if (choosingMarker && !IsMouseOverSpecificUI(gameUI) && IsInMap())
        {
            mouseFollower.SetActive(true);
            mouseFollower.transform.position = Input.mousePosition;
        }
        else if (choosingMarker && (IsMouseOverSpecificUI(gameUI) || !IsInMap()))
        {
            mouseFollower.SetActive(false);
        }
        if (choosingMarker && Input.GetKeyDown(KeyCode.Mouse0) && IsInMap() && !IsMouseOverSpecificUI(gameUI))
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
        if (targets.Count > 0)
        {
            GameObject firstEnemy = targets[0];
            float closestDistance = PathData.realPossiblePaths[targets[0].GetComponent<EnemyAI>().currentPath].Count - targets[0].GetComponent<EnemyAI>().currentWaypoint;
            float closestWaypointDistance = 0;
            if (targets[0].GetComponent<EnemyAI>().currentWaypoint < PathData.realPossiblePaths[targets[0].GetComponent<EnemyAI>().currentPath].Count)
            {
                closestWaypointDistance = Vector3.Distance(PathData.realPossiblePaths[targets[0].GetComponent<EnemyAI>().currentPath][targets[0].GetComponent<EnemyAI>().currentWaypoint], targets[0].transform.position);
            }
            else
            {
                closestWaypointDistance = 0;
            }
            for (int i = 0; i < targets.Count; ++i)
            {
                GameObject currentEnemy = targets[i];
                float currentDistance = PathData.realPossiblePaths[targets[i].GetComponent<EnemyAI>().currentPath].Count - targets[i].GetComponent<EnemyAI>().currentWaypoint;
                float currentWaypointDistance = 0;
                if (targets[i].GetComponent<EnemyAI>().currentWaypoint < PathData.realPossiblePaths[targets[i].GetComponent<EnemyAI>().currentPath].Count)
                {
                    currentWaypointDistance = Vector3.Distance(PathData.realPossiblePaths[targets[i].GetComponent<EnemyAI>().currentPath][targets[i].GetComponent<EnemyAI>().currentWaypoint], targets[i].transform.position);
                }
                else
                {
                    currentWaypointDistance = 0;
                }
                if (currentDistance < closestDistance)
                {
                    if (Vector3.Distance(firstEnemy.transform.position, currentEnemy.transform.position) >= smoothnessFactor)
                    {
                        firstEnemy = currentEnemy;
                        closestDistance = currentDistance;
                        closestWaypointDistance = currentWaypointDistance;
                    }
                }
                else if (currentDistance == closestDistance && currentWaypointDistance < closestWaypointDistance)
                {
                    if (Vector3.Distance(firstEnemy.transform.position, currentEnemy.transform.position) >= smoothnessFactor)
                    {
                        firstEnemy = currentEnemy;
                        closestDistance = currentDistance;
                        closestWaypointDistance = currentWaypointDistance;
                    }
                }
            }
            return firstEnemy.transform.position;
        }
        return new Vector3();
    }
    public Vector3 LastEnemy()
    {
        if (targets.Count > 0)
        {
            GameObject lastEnemy = targets[targets.Count - 1];
            float furthestDistance = PathData.realPossiblePaths[targets[targets.Count - 1].GetComponent<EnemyAI>().currentPath].Count - targets[targets.Count - 1].GetComponent<EnemyAI>().currentWaypoint;
            float furthestWaypointDistance = 0;
            if (targets[0].GetComponent<EnemyAI>().currentWaypoint < PathData.realPossiblePaths[targets[targets.Count - 1].GetComponent<EnemyAI>().currentPath].Count)
            {
                furthestWaypointDistance = Vector3.Distance(PathData.realPossiblePaths[targets[targets.Count - 1].GetComponent<EnemyAI>().currentPath][targets[targets.Count - 1].GetComponent<EnemyAI>().currentWaypoint], targets[targets.Count - 1].transform.position);
            }
            else
            {
                furthestWaypointDistance = 0;
            }
            for (int i = 0; i < targets.Count; ++i)
            {
                GameObject currentEnemy = targets[i];
                float currentDistance = PathData.realPossiblePaths[targets[i].GetComponent<EnemyAI>().currentPath].Count - targets[i].GetComponent<EnemyAI>().currentWaypoint;
                float currentWaypointDistance = 0;
                if (targets[i].GetComponent<EnemyAI>().currentWaypoint < PathData.realPossiblePaths[targets[i].GetComponent<EnemyAI>().currentPath].Count)
                {
                    currentWaypointDistance = Vector3.Distance(PathData.realPossiblePaths[targets[i].GetComponent<EnemyAI>().currentPath][targets[i].GetComponent<EnemyAI>().currentWaypoint], targets[i].transform.position);
                }
                else
                {
                    currentWaypointDistance = 0;
                }
                if (currentDistance > furthestDistance)
                {
                    if (Vector3.Distance(lastEnemy.transform.position, currentEnemy.transform.position) >= smoothnessFactor)
                    {
                        lastEnemy = currentEnemy;
                        furthestDistance = currentDistance;
                        furthestWaypointDistance = currentWaypointDistance;
                    }
                }
                else if (currentDistance == furthestDistance && currentWaypointDistance > furthestWaypointDistance)
                {
                    if (Vector3.Distance(lastEnemy.transform.position, currentEnemy.transform.position) >= smoothnessFactor)
                    {
                        lastEnemy = currentEnemy;
                        furthestDistance = currentDistance;
                        furthestWaypointDistance = currentWaypointDistance;
                    }
                }
            }
            return lastEnemy.transform.position;
        }
        return new Vector3();
    }
    public Vector3 TargetMarker() 
    {
        changeMarkerButton.SetActive(true);
        hasMarker = true;
        if (currentMarker == null)
        {
            Vector3 realWorldSpawnPos = new Vector3(transform.position.x - 0.01f, transform.position.y, transform.position.z);
            Vector3 spawnPos = Camera.main.WorldToScreenPoint(realWorldSpawnPos); //make it smarter so that it aims at path or enemy when placed or something like that
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
        GetComponent<TowerMenuScript>().xButton.SetActive(true);
        GetComponent<TowerMenuScript>().xButton.GetComponent<Button>().onClick.AddListener(delegate { CancelNewMarker(); });
    }
    public void PlaceChooseMarker()
    {
        choosingMarker = false;
        GetComponent<TowerMenuScript>().towerMenu.SetActive(true);
        currentMarker.transform.position = Input.mousePosition;
        Destroy(mouseFollower);
        GetComponent<TowerMenuScript>().xButton.SetActive(false);
    }
    public void CancelNewMarker()
    {
        choosingMarker = false;
        GetComponent<TowerMenuScript>().towerMenu.SetActive(true);
        Destroy(mouseFollower);
        GetComponent<TowerMenuScript>().xButton.SetActive(false);
    }
    public Vector3 FollowMouse() // Make it take time to follow the mouse, not instant
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
    //1 bulletSpeed = 1 m/s
    //1 enemySpeed straight = 1 m/s
    Vector3 PredictiveAim(GameObject enemy)
    {
        float bs = GetComponent<TowerStats>().bulletSpeed;
        float ms = enemy.GetComponent<EnemyAI>().movementSpeed;
        float cp = enemy.GetComponent<EnemyAI>().currentPath;
        float cw = enemy.GetComponent<EnemyAI>().currentWaypoint;
        return enemy.transform.position;
    }
    private bool IsMouseOverSpecificUI(Canvas canvas)
    {
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        pointerEventData.position = Input.mousePosition;
        raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
            return true;
        return false;
    }
}
