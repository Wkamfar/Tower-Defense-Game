using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//How do you convert for int to string? Ask next class
public enum difficulty : int
{
    easy = 0,
    medium = 1,
    hard = 2
}
public enum tiles
{ 
    entrance = -1,
    exit = -2,
    path = -3,
    beacon = -4,
    grass = 0,
    crystal = 1,
    plutonium = 2,
    coal = 3,
    iron = 4,
    gold = 5,
    water = 6,
    lava = 7,
    tar = 8,
    oil = 9,
    radioactive_water = 10,
    tree = 11
}
public enum tileSpawnRates
{
    //solids
    grass = 9700,
    crystal = 25,
    plutonium = 10,
    coal = 150,
    iron = 75,
    gold = 40,
    //liquids
    water = 3000,
    lava = 2000,
    tar = 1500,
    oil = 2500,
    radioactive_water = 1000
}
public class Info 
{
    public difficulty currentDifficulty = difficulty.easy;
}
