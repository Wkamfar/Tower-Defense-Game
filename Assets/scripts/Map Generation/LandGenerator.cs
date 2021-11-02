using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGenerator
{
    public int[,] LandGeneration(int[,] array)
    {
        for (int y = 0; y < array.GetLength(0); y++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                if (array[y, x] == 0)
                {
                    array[y, x] = RandomTile();
                }
            }
        }
        return array;
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
