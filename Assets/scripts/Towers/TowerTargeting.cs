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
    public Vector3 marker;
    private List<GameObject> targets = new List<GameObject>();
    private void Update()
    {
        targets = GetComponent<TowerStats>().targets;
    }
    public Vector3 targeting(int mode)
    {
        hasMarker = false;
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
        hasMarker = true; //This is more complicated than I thought, do this later // You need to add a placable UI marker that appears with the menu // You need to add a button that hides the UI and allows you to place the target for the tower, and with that a way to exit back to the menu
        if (GetComponent<TowerMenuScript>().towerMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            marker = new Vector3(mousePos.x, transform.position.y, mousePos.z);
        }
        return marker;
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
}
