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
    public void GeneratePath()
    { // 1 = y, 0 = x
        int currentX = MapData.startingPoint[0];
        int currentY = MapData.startingPoint[1];
        for (currentX = 1; currentX < MapData.xLength - 1; ++currentX) // ++x is faster than x++, but if x = 1 ++x would make it 2 and then use the value of x, and x++ would use the value of x and then make it 2
        {
            if (currentY > MapData.endingPoint[1])
            {
                --currentY;
            }
            else if (currentY < MapData.endingPoint[1])
            {
                ++currentY;
            }
            GeneratePathTile(currentX, currentY);
        }
        SavePath();
    }
    public void GeneratePath2(int min, int max, int cooldown)
    { // 1 = y, 0 = x
        int currentX = MapData.startingPoint[0];
        int currentY = MapData.startingPoint[1];
        int yDirection;
        int gap = cooldown;
        for (currentX = 1; currentX < MapData.xLength - 2; ++currentX) // ++x is faster than x++, but if x = 1 ++x would make it 2 and then use the value of x, and x++ would use the value of x and then make it 2
        {
            yDirection = Random.Range(0, 3);
            yDirection--;
            int yDisplacement = Random.Range(min, max);
            gap = gap <= 0 ? -1 : gap;
            for (;yDisplacement > 0; currentY += yDirection)
            {
                if (currentY < 0)
                {
                    currentY = 0;
                    yDisplacement = 0;
                }
                if (currentY >= MapData.yLength)
                {
                    currentY = MapData.yLength - 1;
                    yDisplacement = 0;
                }
                GeneratePathTile(currentX, currentY);
                if (yDirection == 0 || gap > 0)
                {
                    --gap;
                    currentY += yDirection;
                    break;
                }
                --yDisplacement;
            }
            currentY -= yDirection;
            gap = gap <= -1 ? cooldown : gap;
        }
        yDirection = currentY > MapData.endingPoint[1] ? -1 : 1;
        while (currentY != MapData.endingPoint[1])
        {
            GeneratePathTile(currentX, currentY);
            currentY += yDirection;
        }
        SavePath();
    }
    private void GeneratePathTile(int currentX, int currentY)
    {
        PathData.pathLocations.Add(new List<int>() { currentX, currentY });
        MapData.UpdateGrid(new List<int>() { currentX, currentY }, tiles.path);
    }
    private void SavePath()
    {
        PathData.possiblePaths.Add(PathData.pathLocations);
        PathData.pathLocations.Clear();
    }
}
