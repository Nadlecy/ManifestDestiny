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
    public static CustomMaths cMaths;
    public List<Seraph> playerTeam;

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
        display.SetPlayerPosition(15,15);

        while (true)
        {
            keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                display.PlayerWorldDisplay(0, -1);
            } else if(keyInfo.Key == ConsoleKey.RightArrow)
            {
                display.PlayerWorldDisplay(0, 1);
            } else if(keyInfo.Key == ConsoleKey.UpArrow)
            {
                display.PlayerWorldDisplay(-1, 0);
            } else if(keyInfo.Key == ConsoleKey.DownArrow)
            {
                display.PlayerWorldDisplay(1, 0);
            } else if(keyInfo.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }
}
