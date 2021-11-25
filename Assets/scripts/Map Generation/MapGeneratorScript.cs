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
    public GameObject path;
    public GameObject[] blocks;
    public GameObject beacon;
    //Make a script to store all this or make it random or do something else with it
    public int structCount;
    public int minYSize;
    public int maxYSize;
    public int minXSize;
    public int maxXSize;
    public Vector2 spawningPoint;
    private PathGenerator pathgenerator;
    private LandmarkGenerator landmarkgenerator;
    private LandGenerator landgenerator;
    private WorldBuilder worldbuilder;
    void Start()
    {
        MapData.mapCenter = spawningPoint;
        pathgenerator = new PathGenerator();
        landmarkgenerator = new LandmarkGenerator();
        landgenerator = new LandGenerator();
        worldbuilder = new WorldBuilder();
        GenerateMap();
    }

    void GenerateMap()
    {
        CreateGrid(xSize, ySize);
        //pathgenerator.GeneratePath();
        pathgenerator.GeneratePath2(5, 10, 5);
        pathgenerator.GeneratePath2(2, 7, 3);
        //pathgenerator.GeneratePath2(10, 4, 1);
        landmarkgenerator.StructureGenerator(structCount, minYSize, maxYSize, minXSize, maxXSize);
        landgenerator.LandGeneration();
        //Add forest generation later
        PlaceBeacon(3);
        worldbuilder.SpawnBlocks(blocks, path, beacon, MapData.mapCenter, offset);
        MapData.isMapSpawned = true;
    }
    public static void CreateGrid(int _xLength, int _yLength)
    {
        MapData.xLength = _xLength;
        MapData.yLength = _yLength;
        for (int y = 0; y < _yLength; ++y)
        {
            List<int> row = new List<int>();
            for (int x = 0; x < _xLength; ++x)
            {
                row.Add(0);
            }
            MapData.grid.Add(row);
        }
        
        MapData.startingPoint.Add(0);
        MapData.startingPoint.Add(Random.Range(0, MapData.yLength));
        MapData.endingPoint.Add(MapData.xLength - 1);
        MapData.endingPoint.Add(Random.Range(0, MapData.yLength));
        MapData.UpdateGrid(MapData.startingPoint, tiles.entrance);
        MapData.UpdateGrid(MapData.endingPoint, tiles.exit);
        MapData.isGridCreated = true;
    }
    private void PlaceBeacon(int size)
    {
        int x = Random.Range(0, MapData.grid[0].Count);
        int y = Random.Range(0, MapData.grid.Count);
        while (!CheckArea(x, y, size))
        {
            x = Random.Range(0, MapData.grid[0].Count);
            y = Random.Range(0, MapData.grid.Count);
        }
        MapData.grid[(2 * y + size - 1)/2][(2 * x + size - 1) / 2] = -4;
    }
    private bool CheckArea(int x, int y, int size)
    {
        if (x + size - 1 >= MapData.grid[0].Count )
        {
            return false;
        }
        if (y + size - 1 >= MapData.grid.Count)
        {
            return false;
        }
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                if (MapData.grid[y + i][x + j] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
