using ManifestDestiny;
using ManifestDestiny.Helper.Math;
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
    public enum GameStates
    {
        Exploration,
        Battle,
        Menu,
    }

    public ConsoleKeyInfo keyInfo;
    public static Random rand;
    public static CustomMaths cMaths;
    public GameStates GameState { get; set;}
    public List<Seraph> playerTeam;
    public bool MenuOpen { get; set; }
    public string Selection { get; set; }
    public ItemStorage Inventory { get; set; }

    public GameManager()
    {
        rand = new Random();
        GameState = GameStates.Exploration;
    }

    public void GameLoop()
    {
        WorldMap worldMap = new WorldMap();
        worldMap.SetMap("Map01.txt");
        Display display = new Display();
        display.SetWorldDisplay(worldMap.WorldMapTiles);
        display.WorldDisplay();
        display.SetPlayerPosition(15, 15);

        MenuOpen = false;

        Menu mainMenu = new Menu("MAIN MENU", new List<string> { "SERAPHIM", "BAG", "QUIT GAME", "CLOSE" });

        while (true)
        {
            keyInfo = Console.ReadKey();
            Selection = null;


            /*
            if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                display.PlayerWorldDisplay(-1, 0);
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                display.PlayerWorldDisplay(1, 0);
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                break;
            }
            */

            switch (GameState)
            {
                case GameStates.Exploration:
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            display.PlayerWorldDisplay(0, -1);
                            break;
                        case ConsoleKey.RightArrow:
                            display.PlayerWorldDisplay(0, 1);
                            break;
                        case ConsoleKey.UpArrow:
                            display.PlayerWorldDisplay(-1, 0);
                            break;
                        case ConsoleKey.DownArrow:
                            display.PlayerWorldDisplay(1, 0);
                            break;
                    }
                    break;
                case GameStates.Menu:
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            break;
                        case ConsoleKey.RightArrow:
                            break;
                        case ConsoleKey.UpArrow:
                            mainMenu.PreviousLine();
                            display.MenuDisplay(mainMenu); // Update display
                            break;
                        case ConsoleKey.DownArrow:
                            mainMenu.NextLine();
                            display.MenuDisplay(mainMenu); // Update display
                            break;
                        case ConsoleKey.Enter:
                            Selection = mainMenu.Enter();
                            break;
                        case ConsoleKey.Escape:
                            //mainMenu.Back();
                            break;
                    }   
                    break;

                default:
                    break;
            }
            /*
            // Main Menu
            switch (Selection)
            {
                case "BAG":
                    MenuOpen = false;
                    display.WorldDisplay();
                    display.BagDisplay();
                    break;
                case "CLOSE":
                    MenuOpen = false;
                    display.WorldDisplay();
                    break;
                case "SAVE AND QUIT GAME":
                    // TODO
                    break;
            }*/ 
            // MOVE THIS TO MENU CLASS
        }
    }
}
