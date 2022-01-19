using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SolarLaserSpecialItemScript : TowerSpecialItemScript
{
    protected override bool CanPlaceTower()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int xPoint = Mathf.RoundToInt((mousePos.x - MapData.offset[0]) / MapData.offset[2]);
        int yPoint = Mathf.RoundToInt((mousePos.z - MapData.offset[1]) / -MapData.offset[2]);
        Vector3 realWorldPos = MapData.PointToRealWorld(new List<int>() { xPoint, yPoint });
        mouseFollower.transform.position = new Vector3(realWorldPos.x, 1.5f, realWorldPos.z);
        //mouseFollower.transform.position = new Vector3(mousePos.x, 1.5f, mousePos.z);
        if (PlayerData.playerMoney < specialItemCost || !IsInMap() || IsMouseOverUI() || WrongBlock() || TooCloseToOtherTower())
        {
            mouseFollower.SetActive(false);
            return false;
        }
        mouseFollower.SetActive(true);
        //Debug.Log("Can Place Special Item");
        return true;
    }
    protected override void PlaceTower()
    {
        //Debug.Log("SolarLaserSpecialItemScript.PlaceTower: This happened");
        base.PlaceTower();
    } 
    private bool IsInMap()
    {
        float xPoint = (mouseFollower.transform.position.x - MapData.offset[0]) / MapData.offset[2];
        float yPoint = (mouseFollower.transform.position.z - MapData.offset[1]) / -MapData.offset[2];
        //Debug.Log("PlayerActionScript.IsInMap: The xPoint is: " + xPoint + ", and the yPoint is: " + yPoint);
        if (yPoint < MapData.grid.Count && yPoint >= 0 && xPoint < MapData.grid[0].Count && xPoint >= 0)
        {
            return true;
        }
        return false;
    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private bool WrongBlock()
    {
        bool canPlace = false;
        int xPoint = (int)((mouseFollower.transform.position.x - MapData.offset[0]) / MapData.offset[2]);
        int yPoint = (int)((mouseFollower.transform.position.z - MapData.offset[1]) / -MapData.offset[2]);
        for (int i = 0; i < allowedBlocks.Count; ++i)
        {
            if (allowedBlocks[i].GetComponent<BlockStats>().blockNumber == MapData.gameObjectGrid[yPoint][xPoint].GetComponent<BlockStats>().blockNumber)
            {
                canPlace = true;
                break;
            }
        }
        return !canPlace;
    }
    private bool TooCloseToOtherTower()
    {
        bool tooClose = false;
        foreach (GameObject t in TowerData.towers)
        {
            if (t.GetComponent<TowerStats>().hasCapsuleCollider)
            {
                float x = Mathf.Abs(mouseFollower.transform.position.x - t.transform.position.x);
                float y = Mathf.Abs(mouseFollower.transform.position.z - t.transform.position.z);
                float distance = Mathf.Sqrt(x * x + y * y);
                if (distance < GetTowerRadius(mouseFollower, t) + t.GetComponent<TowerStats>().hitbox)
                {
                    tooClose = true;
                }
            }
            else if (t.GetComponent<TowerStats>().hasBoxCollider)
            {
                float towerRadius = 0;
                float x = Mathf.Abs(t.transform.position.x - mouseFollower.transform.position.x);
                float y = Mathf.Abs(t.transform.position.z - mouseFollower.transform.position.z);
                float totalDistance = Mathf.Sqrt(x * x + y * y);
                float radiant = Mathf.Asin(y / totalDistance);
                float angle = radiant * 180 / Mathf.PI;
                if (angle >= 45)
                {
                    towerRadius = t.GetComponent<TowerStats>().hitbox / Mathf.Sin(radiant);
                }
                else
                {
                    towerRadius = t.GetComponent<TowerStats>().hitbox / Mathf.Cos(radiant);
                }
                //Debug.Log("PlayerActionScript.WrongBlock: The radius of the block currently is")
                //Fix all of the issues
                if (towerRadius + GetTowerRadius(mouseFollower, t) > totalDistance)
                {
                    tooClose = true;
                }
            }
        }
        foreach (GameObject i in TowerData.specialItems)
        {
            if (i.GetComponent<SpecialItemStats>().hasCapsuleCollider)
            {
                float x = Mathf.Abs(mouseFollower.transform.position.x - i.transform.position.x);
                float y = Mathf.Abs(mouseFollower.transform.position.z - i.transform.position.z);
                float distance = Mathf.Sqrt(x * x + y * y);
                if (distance < GetTowerRadius(mouseFollower, i) + i.GetComponent<SpecialItemStats>().hitbox)
                {
                    tooClose = true;
                }
            }
            else if (i.GetComponent<SpecialItemStats>().hasBoxCollider)
            {
                float towerRadius = 0;
                float x = Mathf.Abs(i.transform.position.x - mouseFollower.transform.position.x);
                float y = Mathf.Abs(i.transform.position.z - mouseFollower.transform.position.z);
                float totalDistance = Mathf.Sqrt(x * x + y * y);
                float radiant = Mathf.Asin(y / totalDistance);
                float angle = radiant * 180 / Mathf.PI;
                if (angle >= 45)
                {
                    towerRadius = i.GetComponent<SpecialItemStats>().hitbox / Mathf.Sin(radiant);
                }
                else
                {
                    towerRadius = i.GetComponent<SpecialItemStats>().hitbox / Mathf.Cos(radiant);
                }
                //Debug.Log("PlayerActionScript.WrongBlock: The radius of the block currently is")
                //Fix all of the issues
                if (towerRadius + GetTowerRadius(mouseFollower, i) > totalDistance)
                {
                    tooClose = true;
                }
            }
            if (mouseFollower.transform.position == i.transform.position)
            {
                tooClose = true;
            }
        }
        return tooClose;
    }
    private float GetTowerRadius(GameObject tower, GameObject checkObject)
    {
        float towerRadius = 0;
        if (hasCapsuleCollider)
        {
            towerRadius = hitbox;
        }
        else if (hasBoxCollider)
        {
            float x = Mathf.Abs(checkObject.transform.position.x - tower.transform.position.x);
            float y = Mathf.Abs(checkObject.transform.position.z - tower.transform.position.z);
            float totalDistance = Mathf.Sqrt(x * x + y * y);
            float radiant = Mathf.Asin(y / totalDistance);
            float angle = radiant * 180 / Mathf.PI;
            if (angle >= 45)
            {
                towerRadius = hitbox / Mathf.Sin(radiant);
            }
            else
            {
                towerRadius = hitbox / Mathf.Cos(radiant);
            }
        }
        return towerRadius;
    }
}
