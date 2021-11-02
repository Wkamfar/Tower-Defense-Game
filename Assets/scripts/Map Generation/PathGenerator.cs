using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator
{
    // left to right path generation
    // add in difficulty later
    //generate a random starting point on the left side and then on the right side
    // add the start and end point to path data later, I don't really know how to do it myself
    //improve the generation AI later
    //can add left later, doesn't need it now
    // make a path list here and upload it to path data later
    public int[,] GeneratePath(int[,] array)
    {
        Vector2 startingPoint = new Vector2(0, Random.Range(0, array.GetLength(0)));
        Vector2 endingPoint = new Vector2(array.GetLength(1) - 1, Random.Range(0, array.GetLength(0)));
        Vector2 pathbuilderLocation = new Vector2(startingPoint.x, startingPoint.y);
        array[(int)startingPoint.y, (int)startingPoint.x] = (int)tiles.entrance;
        array[(int)endingPoint.y, (int)endingPoint.x] = (int)tiles.exit;
        PathData.pathLocations.Add(startingPoint);
        while (pathbuilderLocation != endingPoint)
        {
            bool[] pathbuilderConditions = new bool[] { pathbuilderLocation.x > 0, 
                                                        pathbuilderLocation.x < array.GetLength(1) - 1,
                                                        pathbuilderLocation.y < array.GetLength(0) - 1,
                                                        pathbuilderLocation.y > 0}; // 0 = right, 1 = up, 2 = down
            int[] xDirection = new int[] {-1, 1, 0, 0 };
            int[] yDirection = new int[] { 0, 0, 1,-1 };
            for (int i = 1; i < 4; i++)
            {
                if (pathbuilderConditions[i])
                {
                    if (array[(int)(pathbuilderLocation.y + yDirection[i]), (int)(pathbuilderLocation.x + xDirection[i])] == 0)
                    {
                        int currentDistance = (int)(Mathf.Abs(endingPoint.x - pathbuilderLocation.x) + Mathf.Abs(endingPoint.y - pathbuilderLocation.y));
                        int moveDistance = (int)(Mathf.Abs(endingPoint.x - pathbuilderLocation.x - xDirection[i]) + Mathf.Abs(endingPoint.y - pathbuilderLocation.y - yDirection[i]));
                        if (currentDistance > moveDistance)
                        {
                            pathbuilderLocation = new Vector2(pathbuilderLocation.x + xDirection[i], pathbuilderLocation.y + yDirection[i]);
                            PathData.pathLocations.Add(pathbuilderLocation);
                            array[(int)pathbuilderLocation.y, (int)pathbuilderLocation.x] = -3;
                            continue;
                        }
                    }
                    else if (array[(int)(pathbuilderLocation.y + yDirection[i]), (int)(pathbuilderLocation.x + xDirection[i])] == -2)
                    {
                        pathbuilderLocation = endingPoint;
                        PathData.pathLocations.Add(endingPoint);
                        continue;
                    }
                }
            }
        }
        return array;
    }
    public void GeneratePath2(int[,] map)
    { // 1 = y, 0 = x
        List<int> startingPoint = new List<int>() { 0, Random.Range(0, map.GetLength(0)) };
        List<int> endingPoint = new List<int>() { map.GetLength(1) - 1, Random.Range(0, map.GetLength(0)) };
        AddToPath(map, startingPoint, tiles.entrance);
        AddToPath(map, endingPoint, tiles.exit);
        int xSize = map.GetLength(1);
        int ySize = map.GetLength(0);
        int currentX = startingPoint[0];
        int currentY = startingPoint[1];
        for (currentX = 1; currentX < xSize; ++currentX) // ++x is faster than x++, but if x = 1 ++x would make it 2 and then use the value of x, and x++ would use the value of x and then make it 2
        {
            
            if (currentY > endingPoint[1])
            {
                --currentY;
            }
            else if (currentY < endingPoint[1])
            {
                ++currentY;
            }
            AddToPath(map, new List<int>() { currentX, currentY }, tiles.path);

        }
    }
    void AddToPath(int[,] map, List<int> point, tiles tile)
    {
        PathData.listLocations.Add(point);
        map[point[1], point[0]] = (int)tile;
    }
}
