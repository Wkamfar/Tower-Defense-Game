using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGenerator
{
    public void LandGeneration()
    {
        for (int y = 0; y < MapData.yLength; y++)
        {
            for (int x = 0; x < MapData.xLength; x++)
            {
                if (MapData.grid[y][x] == 0)
                {
                    MapData.grid[y][x] = RandomTile();
                }
            }
        }
    }
    int RandomTile()
    {
        float[] tilePercentages = new float[] { (float)tileSpawnRates.grass, (float)tileSpawnRates.crystal, (float)tileSpawnRates.plutonium, (float)tileSpawnRates.coal, (float)tileSpawnRates.iron, (float)tileSpawnRates.gold};
        int[] tileNumbers = new int[] { (int)tiles.grass, (int)tiles.crystal, (int)tiles.plutonium, (int)tiles.coal, (int)tiles.iron, (int)tiles.gold };
        float randomPercentage = Random.Range(0, 10000);
        for (int i = 0; i < tilePercentages.Length; ++i)
        {
            randomPercentage -= tilePercentages[i];
            if (randomPercentage < 0)
            {
                return tileNumbers[i];
            }
        }
        return (int)tiles.grass;
    }
}
