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
        float[] tilePercentages = new float[] { (float)tileSpawnRates.grass / 100, (float)tileSpawnRates.crystal / 100, (float)tileSpawnRates.plutonium / 100, (float)tileSpawnRates.coal / 100, (float)tileSpawnRates.iron / 100, (float)tileSpawnRates.gold / 100 };
        int[] tileNumbers = new int[] { (int)tiles.grass, (int)tiles.crystal, (int)tiles.plutonium, (int)tiles.coal, (int)tiles.iron, (int)tiles.gold };
        float randomPercentage = Random.Range(0, 100);
        for (int i = 0; i < tilePercentages.Length; i++)
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
