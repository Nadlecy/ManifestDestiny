using ManifestDestiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameManager
{
    enum GameStates {
        Exploration,
        Battle,
        Menu,
    }
    
    public ConsoleKeyInfo keyInfo;
    public static Random rand;

    public GameManager()
    {
        rand = new Random();
        GameStates gameState = GameStates.Exploration;
    }

    public void GameLoop()
    {
        WorldMap worldMap = new WorldMap();
        worldMap.Init();
        worldMap.SetMap("Map01.txt");
        Display display = new Display();
        display.SetWorldDisplay(worldMap.WorldMapTiles);
        display.WorldDisplay();

        while (true)
        {
            keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                display.PlayerWorldDisplay(10, 10);
            }
        }
    }
}
