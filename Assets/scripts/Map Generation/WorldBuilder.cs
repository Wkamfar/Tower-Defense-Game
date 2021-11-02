using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public void SpawnBlocks(int[,] array, GameObject[] blockReferences, GameObject pathReference, Vector2 centerPoint, float offset)
    {
        Vector2 startingLocation = new Vector2(centerPoint.x + array.GetLength(1) / 2 * -offset, centerPoint.y + array.GetLength(0) / 2 * offset);
        Vector2 spawningLocation = new Vector2(startingLocation.x, startingLocation.y);
        for (int y = 0; y < array.GetLength(0); y++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                if (array[y, x] == -3 || array[y, x] == -2 || array[y, x] == -1)
                {
                    Instantiate(pathReference, new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                }
                else
                {
                    Instantiate(blockReferences[array[y,x]], new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                }
                spawningLocation = new Vector2(spawningLocation.x + offset, spawningLocation.y);
            }
            spawningLocation = new Vector2(startingLocation.x, spawningLocation.y - offset);
        }
    }
}
