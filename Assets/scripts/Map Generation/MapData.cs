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
