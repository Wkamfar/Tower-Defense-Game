using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkGenerator
{
    public int[,] StructureGenerator(int[,] array, int structureCount, int minYSize, int maxYSize, int minXSize, int maxXSize)
    {
        for (int i = 0; i < structureCount; i++)
        {
            int ySize = Random.Range(minYSize, maxYSize);
            int xSize = Random.Range(minXSize, maxXSize);
            Vector2[,] successfulSpotArray = FindSpot2(array, ySize, xSize);
            int liquidNumber = DecideLiquid();
            array = FillInArray(array, successfulSpotArray, liquidNumber);
        }
        return array;
    }
    Vector2[,] FindSpot(int[,] array, int ySize, int xSize)
    {
        Vector2 spawningPoint = new Vector2(Random.Range(0, array.GetLength(1)), Random.Range(0, array.GetLength(0)));
        Vector2 spaceChecker = new Vector2(spawningPoint.x, spawningPoint.y);
        Vector2 endPoint = new Vector2();
        int[] xDirection = new int[] { -1, 1, 0, 0 };
        int[] yDirection = new int[] { 0, 0, 1, -1 };

        int[,] xFillDirection = new int[,] { { 0, 0, ySize - 1, ySize - 1 }, { 0, 0, -ySize + 1, -ySize + 1 } };
        int[,] yFillDirection = new int[,] { { ySize - 1, ySize - 1, 0, 0 }, { -ySize + 1, -ySize + 1, 0, 0 } };

        int[,] xSearchDirection = new int[,] { { 0, 0, 1, 1 }, { 0, 0, -1, -1 } };
        int[,] ySearchDirection = new int[,] { { 1, 1, 0, 0 }, { -1, -1, 0, 0 } };
        Vector2[,] currentSpot = new Vector2[ySize, xSize];
        for (int j = 0; j < 4; j++)
        {
            for (int k = 0; k < xSize; k++)
            {
                bool[] directionConditions = new bool[] { spaceChecker.x > 0, spaceChecker.x < array.GetLength(1) - 1, spaceChecker.y < array.GetLength(0) - 1, spaceChecker.y > 0 }; //left = 0, right = 1, up = 2, down = 3
                if (directionConditions[j] && array[(int)(spaceChecker.y + yDirection[j]), (int)(spaceChecker.x + xDirection[j])] == 0)
                {
                    spaceChecker = new Vector2(spaceChecker.x + xDirection[j], spaceChecker.y + yDirection[j]);
                    endPoint = new Vector2(spaceChecker.x, spaceChecker.y);
                }
                else
                {
                    break;
                }
                if (k == xSize - 1)
                {
                    for (int m = 0; m < 2; m++)
                    {
                        bool abortSearch = false;
                        Vector2 fillLocation = new Vector2(spaceChecker.x + xFillDirection[m, j], spaceChecker.y + yFillDirection[m, j]);
                        if (fillLocation.x > 0 && fillLocation.x < array.GetLength(1) && fillLocation.y < array.GetLength(0) && fillLocation.y > 0)
                        {
                            spaceChecker = new Vector2(spawningPoint.x, spawningPoint.y);
                            for (int y = 0; y < ySize; y++)
                            {
                                for (int x = 0; x < xSize; x++)
                                {
                                    currentSpot[y, x] = new Vector2(spaceChecker.x, spaceChecker.y);
                                    spaceChecker = new Vector2(spaceChecker.x + xSearchDirection[m, j], spaceChecker.y);
                                        
                                    if (array[(int)spaceChecker.y, (int)spaceChecker.x] != 0)
                                    {
                                        abortSearch = true;
                                        break;
                                    }
                                }
                                spaceChecker = new Vector2(spaceChecker.x, spaceChecker.y + ySearchDirection[m, j]);
                                if (abortSearch)
                                {
                                    break;
                                }
                            }
                        }
                        if (abortSearch)
                        {
                            continue;
                        }
                        return currentSpot;
                    }
                }
            }
        }
        return new Vector2[,] { };
    }
    Vector2[,] FindSpot2(int[,] array, int ySize, int xSize)
    {
        Vector2 spawningLocation = new Vector2(Random.Range(0, array.GetLength(1)), Random.Range(0, array.GetLength(0)));
        Vector2 spaceChecker = new Vector2(spawningLocation.x, spawningLocation.y);
        Vector2[,] currentSpot = new Vector2[ySize, xSize];
        int[] xDirection = new int[] { -1, -1, -1, -1, 1, 1, 1, 1};
        int[] yDirection = new int[] { 1, 1, -1, -1, -1, -1, 1, 1};
        //check the whole box, and fill it what can be filled in
        //All the number meaning
        // 0 = (spawningLocation.x - ySize, spawningLocation.y + xSize)
        // 1 = (spawningLocation.x - xSize, spawningLocation.y + ySize)
        // 2 = (spawningLocation.x - xSize, spawningLocation.y - ySize)
        // 3 = (spawningLocation.x - ySize, spawningLocation.y - xSize)
        // 4 = (spawningLocation.x + ySize, spawningLocation.y - xSize)
        // 5 = (spawningLocation.x + xSize, spawningLocation.y - ySize)
        // 6 = (spawningLocation.x + xSize, spawningLocation.y + ySize)
        // 7 = (spawningLocation.x + ySize, spawningLocation.y + xSize)
        for (int i = 0; i < 8; i++)
        {
            bool abortSearch = false;
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    currentSpot[y, x] = spaceChecker;
                    spaceChecker = new Vector2(spaceChecker.x + xDirection[i], spaceChecker.y);
                    if (spaceChecker.x < 0 || spaceChecker.x >= array.GetLength(0) || spaceChecker.y < 0 || spaceChecker.y >= array.GetLength(1) || array[(int)spaceChecker.y, (int)spaceChecker.x] != 0)
                    {
                        abortSearch = true;
                        break;
                    }
                }
                spaceChecker = new Vector2(spawningLocation.x, spaceChecker.y + yDirection[i]);
                if (abortSearch)
                {
                    break;
                }
            }
            if (abortSearch)
            {
                continue;
            }
            return currentSpot;
        }
        return new Vector2[,] { };
    }
    int[,] FillInArray(int[,] array, Vector2[,] fillSpots, int liquid)
    {
        for (int y = 0; y < fillSpots.GetLength(0); y++)
        {
            for (int x = 0; x < fillSpots.GetLength(1); x++)
            {
                array[(int)fillSpots[y, x].y, (int)fillSpots[y, x].x] = liquid;
            }
        }
        return array;
    }
    int DecideLiquid()
    {
        float[] liquidPercentages = new float[] { (float)tileSpawnRates.water / 100, (float)tileSpawnRates.lava / 100, (float)tileSpawnRates.tar / 100, (float)tileSpawnRates.oil / 100, (float)tileSpawnRates.radioactive_water / 100 };
        int[] liquidNumbers = new int[] { (int)tiles.water, (int)tiles.lava, (int)tiles.tar, (int)tiles.oil, (int)tiles.radioactive_water };
        float randomPercentage = Random.Range(0, 100);
        for (int i = 0; i < liquidNumbers.Length; i++)
        {
            randomPercentage -= liquidPercentages[i];
            if (randomPercentage < 0)
            {
                return liquidNumbers[i];
            }
        }
        return (int)tiles.water;
    }
}
