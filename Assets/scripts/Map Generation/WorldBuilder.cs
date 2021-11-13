using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public void SpawnBlocks(GameObject[] blockReferences, GameObject pathReference, Vector2 centerPoint, float offset)
    {
        Vector2 startingLocation = new Vector2(centerPoint.x + MapData.xLength / 2 * -offset, centerPoint.y + MapData.yLength / 2 * offset);
        Vector2 spawningLocation = new Vector2(startingLocation.x, startingLocation.y);
        CalculateOffset(startingLocation.x, startingLocation.y, offset);
        ConvertPathToReal();
        for (int y = 0; y < MapData.yLength; y++)
        {
            for (int x = 0; x < MapData.xLength; x++)
            {

                if (MapData.grid[y][x] == (int)tiles.path || MapData.grid[y][x] == (int)tiles.exit || MapData.grid[y][x] == (int)tiles.entrance)
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
    private void CalculateOffset(float x, float y, float d)
    {
        MapData.offset.Add(x);
        MapData.offset.Add(y);
        MapData.offset.Add(d);
    }
    private void ConvertPathToReal()
    {
        int currentPath = 0;
        for (int i = 0; i < PathData.possiblePaths.Count; i++)
        {
            PathData.realPossiblePaths.Add(new List<Vector3>());
            for (int j = 0; j < PathData.possiblePaths[i].Count; j++)
            {
                PathData.realPossiblePaths[currentPath].Add(MapData.PointToRealWorld(PathData.possiblePaths[i][j]));
            }
            currentPath++;
        }
    }
}
