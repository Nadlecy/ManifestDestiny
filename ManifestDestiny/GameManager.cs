using ManifestDestiny;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public bool MenuOpen {  get; set; }
    public string Selection { get; set; }

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

        MenuOpen = false;

        Menu mainMenu = new Menu("MainMenu", new List<string> { "CLOSE", "SAVE AND QUIT GAME" });

        while (true)
        {
            keyInfo = Console.ReadKey();
            Selection = null;

            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    display.PlayerWorldDisplay(10, 10);
                    break;
                case ConsoleKey.DownArrow:
                    if (MenuOpen)
                    {
                        mainMenu.NextLine();
                        display.MenuDisplay(mainMenu); // Update display
                    } else
                    {
                        // walk
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (MenuOpen)
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
                    Selection = mainMenu.Enter();
                    break;
                case ConsoleKey.Escape:
                    MenuOpen = !MenuOpen;
                    // Open menu
                    if (MenuOpen == true)
                    {
                        display.MenuDisplay(mainMenu);
                    }
                    else
                    {
                        display.WorldDisplay();
                    }

                    //Console.WriteLine(MenuOpen);
                    break;
                default: break;
            }

            switch (Selection)
            {
                case "CLOSE":
                    MenuOpen = false;
                    display.WorldDisplay();
                    break;
                case "SAVE AND QUIT GAME":
                    // TODO
                    break;
            }
        }
    }
}
