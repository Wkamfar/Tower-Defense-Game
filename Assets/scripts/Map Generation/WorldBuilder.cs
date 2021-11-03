using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public void SpawnBlocks(GameObject[] blockReferences, GameObject pathReference, Vector2 centerPoint, float offset)
    {
        Vector2 startingLocation = new Vector2(centerPoint.x + MapData.xLength / 2 * -offset, centerPoint.y + MapData.yLength / 2 * offset);
        Vector2 spawningLocation = new Vector2(startingLocation.x, startingLocation.y);
        for (int y = 0; y < MapData.yLength; y++)
        {
            for (int x = 0; x < MapData.xLength; x++)
            {
                if (MapData.grid[y][x] == -3 || MapData.grid[y][x] == -2 || MapData.grid[y][x] == -1)
                {
                    Instantiate(pathReference, new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                }
                else
                {
                    Instantiate(blockReferences[MapData.grid[y][x]], new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                }
                spawningLocation = new Vector2(spawningLocation.x + offset, spawningLocation.y);
            }
            spawningLocation = new Vector2(startingLocation.x, spawningLocation.y - offset);
        }
    }
}
