using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public void SpawnBlocks(GameObject[] blockReferences, GameObject pathReference, GameObject beaconReference, Vector2 centerPoint, float offset)
    {
        Vector2 startingLocation = new Vector2(centerPoint.x - MapData.xLength / 2 * offset, centerPoint.y + MapData.yLength / 2 * offset);
        //Debug.Log("WorldBuilder.SpawningBlocks: startingLocation is: (" + startingLocation.x + ", " + startingLocation.y + ")");
        Vector2 spawningLocation = new Vector2(startingLocation.x, startingLocation.y);
        CalculateOffset(startingLocation.x, startingLocation.y, offset);
        ConvertPathToReal();
        for (int y = 0; y < MapData.yLength; ++y)
        {
            List<GameObject> row = new List<GameObject>();
            for (int x = 0; x < MapData.xLength; ++x)
            {
                GameObject currentBlock;
                if (MapData.grid[y][x] == (int)tiles.path || MapData.grid[y][x] == (int)tiles.exit || MapData.grid[y][x] == (int)tiles.entrance)
                {
                    currentBlock = Instantiate(pathReference, new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                }
                else if (MapData.grid[y][x] == (int)tiles.beacon)
                {
                    currentBlock = Instantiate(blockReferences[0], new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                    GameObject currentBeacon = Instantiate(beaconReference, new Vector3(spawningLocation.x, 1.5f, spawningLocation.y), Quaternion.identity);
                }
                else
                {
                    currentBlock = Instantiate(blockReferences[MapData.grid[y][x]], new Vector3(spawningLocation.x, 0, spawningLocation.y), Quaternion.identity);
                }
                spawningLocation = new Vector2(spawningLocation.x + offset, spawningLocation.y);
                row.Add(currentBlock);
                //Debug.Log("WorldBuilder.SpawnBlocks: The grid location of the block is: (" + x + ", " + y + ")");
                //Debug.Log("WorldBuilder.SpawnBlocks: The real world location of the block is: (" + currentBlock.transform.position.x + ", " + currentBlock.transform.position.z + ")");
            }
            spawningLocation = new Vector2(startingLocation.x, spawningLocation.y - offset);
            MapData.gameObjectGrid.Add(row);
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
