using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmarkGenerator
{
    public void StructureGenerator(int structureCount, int minYSize, int maxYSize, int minXSize, int maxXSize)
    {
        for (int i = 0; i < structureCount; i++)
        {
            int ySize = Random.Range(minYSize, maxYSize);
            int xSize = Random.Range(minXSize, maxXSize);
            tiles liquid = DecideLiquid();
            CreateLandmark(ySize, xSize, liquid);
        }
    }
    void CreateLandmark(int ySize, int xSize, tiles type)
    {
        List<int> spawningLocation = new List<int>() { Random.Range(0, MapData.xLength), Random.Range(0, MapData.yLength) };
        for (int y = spawningLocation[1] - ySize/2; y < spawningLocation[1] + ySize/2; ++y)
        {
            for (int x = spawningLocation[0] - xSize/2; x < spawningLocation[0] + xSize/2; ++x)
            {
                if (y >= 0 && y < MapData.yLength && x >= 0 && x < MapData.xLength)
                {
                    MapData.UpdateGridAroundPath(new List<int>() { x, y }, type);
                }
            }
        }
    }
    tiles DecideLiquid()
    {
        float[] liquidPercentages = new float[] { (float)tileSpawnRates.water / 100, (float)tileSpawnRates.lava / 100, (float)tileSpawnRates.tar / 100, (float)tileSpawnRates.oil / 100, (float)tileSpawnRates.radioactive_water / 100 };
        tiles[] liquidNumbers = new tiles[] { tiles.water, tiles.lava, tiles.tar, tiles.oil, tiles.radioactive_water };
        float randomPercentage = Random.Range(0, 100);
        for (int i = 0; i < liquidNumbers.Length; i++)
        {
            randomPercentage -= liquidPercentages[i];
            if (randomPercentage < 0)
            {
                return liquidNumbers[i];
            }
        }
        return tiles.water;
    }
}
