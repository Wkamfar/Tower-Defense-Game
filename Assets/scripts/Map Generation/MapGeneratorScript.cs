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
        //pathgenerator.GeneratePath2(5, 10, 5);
         pathgenerator.GeneratePath2(2, 7, 3);
        landmarkgenerator.StructureGenerator(structCount, minYSize, maxYSize, minXSize, maxXSize);
        landgenerator.LandGeneration();
        //Add forest generation later
        worldbuilder.SpawnBlocks(blocks, path, spawningPoint, offset);
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
}
