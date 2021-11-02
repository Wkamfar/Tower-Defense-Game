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
        GenerateMap(map);
    }

    void GenerateMap(int[,] array)
    {
        array = new int[ySize, xSize];
        pathgenerator.GeneratePath2(array);
        array = landmarkgenerator.StructureGenerator(array, structCount, minYSize, maxYSize, minXSize, maxXSize);
        array = landgenerator.LandGeneration(array);
        //Add forest generation later
        worldbuilder.SpawnBlocks(array, blocks, path, spawningPoint, offset);
    }
}
