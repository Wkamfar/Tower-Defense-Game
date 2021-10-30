using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorScript : MonoBehaviour
{
    private int[,] map;
    public int xSize;
    public int ySize;
    public float offset;
    private Vector2 spawnerLocation;
    public GameObject grass; //make everything grass first
    public GameObject path; // choose an end on both sides and render it in after
    public GameObject water; //put in a water block and expand so it is procedural
    public GameObject[] blocks; 

    // You can make zeros for blank territory
    
    // Make an array first, and then create the map
    void Start()
    {
        GenerateMap(map);
    }

    void GenerateMap(int[,] array)
    {
        array = new int[ySize, xSize];
        for (int y = 0; y < array.GetLength(0); y++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                array[y, x] = 0;
            }
        }

        //Load in the blocks
        spawnerLocation = new Vector2(xSize * offset / 2, ySize * -offset / 2);
        Debug.Log("The spawnlocation is: (" + spawnerLocation.x + ", " + spawnerLocation.y + ")");
    }
}
