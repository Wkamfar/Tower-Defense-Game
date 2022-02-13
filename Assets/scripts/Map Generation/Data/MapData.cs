using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapData 
{
    public static bool isGridCreated = false;
    public static List<int> startingPoint = new List<int>();
    public static List<int> endingPoint = new List<int>();
    public static List<List<int>> grid = new List<List<int>>();
    public static int xLength;
    public static int yLength;
    public static List<float> offset = new List<float>();
    public static float spawnHeight = 2f; //add some value here later
    public static bool isMapSpawned = false;
    public static List<List<GameObject>> gameObjectGrid = new List<List<GameObject>>();
    public static Vector2 mapCenter;
    public static Vector3 PointToRealWorld(List<int> gridPoint)
    {
        Vector3 realWorldPoint = new Vector3();
        realWorldPoint.x = offset[0] + offset[2] * gridPoint[0];
        realWorldPoint.y = spawnHeight;
        realWorldPoint.z = offset[1] - offset[2] * gridPoint[1];
        return realWorldPoint;
    }
    public static Vector3 PointToRealWorld(int x, int y)
    {
        return PointToRealWorld(new List<int>() { x, y });
    }
    public static void UpdateGrid(List<int> point, tiles tile)
    {
        grid[point[1]][point[0]] = (int)tile;
    }
    public static void UpdateGridAroundPath(List<int> point, tiles tile)
    {
        if (!IsPartOfPath(point))
        {
            grid[point[1]][point[0]] = (int)tile;
        }
    }
    public static bool IsPartOfPath(List<int> point)
    {
        switch(grid[point[1]][point[0]])
        {
            case (int)tiles.entrance:
            case (int)tiles.exit:
            case (int)tiles.path:
                return true;
            default:
                return false;
        }
    }
}
