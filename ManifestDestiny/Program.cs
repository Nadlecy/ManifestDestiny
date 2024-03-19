﻿
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{
    public static Random rnd = new Random();

    static void Main(string[] args)
    {
        WorldMap worldMap = new WorldMap();
        worldMap.Init();
        worldMap.SetMap("Map01.txt");
        Display display = new Display();
        display.SetDisplay(worldMap.WorldMapTiles);
        display.ShowDisplay();
    }
}