using ManifestDestiny;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

        bool menuOpen = false;

        Menu mainMenu = new Menu("MainMenu", new List<string> { "CLOSE", "AMOGUS" });

        while (true)
        {
            keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    display.PlayerWorldDisplay(10, 10);
                    break;
                case ConsoleKey.DownArrow:
                    if (menuOpen)
                    {
                        mainMenu.NextLine();
                        display.MenuDisplay(mainMenu); // Update display
                    } else
                    {
                        // walk
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (menuOpen)
                    {
                        mainMenu.PreviousLine();
                        display.MenuDisplay(mainMenu); // Update display
                    }
                    else
                    {
                        // walk
                    }
                    break;
                case ConsoleKey.Enter:
                    mainMenu.Enter();
                    break;
                case ConsoleKey.Escape:
                    menuOpen = !menuOpen;
                    // Open menu
                    if (menuOpen==true)
                    {
                        display.MenuDisplay(mainMenu); 
                    } else { 
                        display.WorldDisplay(); 
                    }
                    //Console.WriteLine(menuOpen);
                    break;
                default: break;
            }
        }
    }
}
