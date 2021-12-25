using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidShooterSpecialRequirement : TowerSpecialRequirement
{
    //Have a display that shows what liquid this tower will be using both on the tower model, and keep it as a variable and save it in tower stats as well, make some inheritance for tower stats
    [SerializeField] private float maxDistance;
    [SerializeField] private List<GameObject> liquids;
    private List<List<int>> InRangeBlocks(GameObject mouseFollower)
    {
        List<float> centerPoint = new List<float>() { (mouseFollower.transform.position.z - MapData.offset[1]) / -MapData.offset[2], (mouseFollower.transform.position.x - MapData.offset[0]) / MapData.offset[2] };
        List<List<int>> blocks = new List<List<int>>();
        centerPoint[0] = Mathf.Round(centerPoint[0]);
        centerPoint[1] = Mathf.Round(centerPoint[1]);
        if (IsInMap(mouseFollower))
        {
            List<int> startingPoint = new List<int>() { (int)centerPoint[1], (int)centerPoint[0] };
            for (int y = (int)Mathf.Ceil(maxDistance + 1); y >= -Mathf.Ceil(maxDistance + 1); --y)
            {
                for (int x = (int)Mathf.Ceil(maxDistance + 1); x >= -Mathf.Ceil(maxDistance + 1); --x)
                {
                    if (startingPoint[1] + y < MapData.grid.Count &&
                        startingPoint[1] + y >= 0 &&
                        startingPoint[0] + x < MapData.grid[0].Count &&
                        startingPoint[0] + x >= 0)
                    {
                        bool allowedBlock = false;
                        for (int i = 0; i < liquids.Count; ++i)
                        {
                            if (MapData.grid[startingPoint[1] + y][startingPoint[0] + x] == liquids[i].GetComponent<BlockStats>().blockNumber)
                            {
                                allowedBlock = true;
                            }
                        }
                        if (allowedBlock)
                        {
                            blocks.Add(new List<int>() { (int)startingPoint[0] + x, (int)startingPoint[1] + y });
                        }
                    }
                }
            }

        }
        return blocks;
    }
    private bool IsInMap(GameObject mouseFollower)
    {
        float xPoint = (mouseFollower.transform.position.x - MapData.offset[0]) / MapData.offset[2];
        float yPoint = (mouseFollower.transform.position.z - MapData.offset[1]) / -MapData.offset[2];
        if (yPoint < MapData.grid.Count - 1 && yPoint >= 0 && xPoint < MapData.grid[0].Count - 1 && xPoint >= 0)
        {
            return true;
        }
        return false;
    }
    public override bool HasMetSpecialRequirement(GameObject mouseFollower)
    {
        List<List<int>> blocks = InRangeBlocks(mouseFollower);
        float closestDistance = -1;
        GameObject closestBlock = null;
        for (int i = 0; i < blocks.Count; ++i)
        {
            GameObject currentBlock = MapData.gameObjectGrid[blocks[i][1]][blocks[i][0]];
            //Debug.Log("")
            float a = Mathf.Abs(currentBlock.transform.position.x - mouseFollower.transform.position.x);
            float b = Mathf.Abs(currentBlock.transform.position.z - mouseFollower.transform.position.z);
            float distance = Mathf.Sqrt(a * a + b * b);
            closestBlock = closestDistance == -1 ? currentBlock : closestBlock;
            closestBlock = distance < closestDistance ? currentBlock : closestBlock;
            closestDistance = closestDistance == -1 ? distance : Mathf.Min(closestDistance, distance);
        }
        if (closestDistance != -1)
        {
            float blockRadius = 0;
            float x = Mathf.Abs(closestBlock.transform.position.x - mouseFollower.transform.position.x);
            float y = Mathf.Abs(closestBlock.transform.position.z - mouseFollower.transform.position.z);
            float totalDistance = Mathf.Sqrt(x * x + y * y);
            float radiant = Mathf.Asin(y / totalDistance);
            float angle = radiant * 180 / Mathf.PI;
            if (angle >= 45)
            {
                blockRadius = (closestBlock.GetComponent<BlockStats>().blockSize / 2) / Mathf.Sin(radiant);
            }
            else
            {
                blockRadius = (closestBlock.GetComponent<BlockStats>().blockSize / 2) / Mathf.Cos(radiant);
            }
            if (blockRadius + TowerData.selectedTower.GetComponent<TowerStats>().hitbox < maxDistance)
            {
                return true;
            }
        }
        return false;
    }
}