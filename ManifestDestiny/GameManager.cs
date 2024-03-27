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
        StartExploration,
        Exploration,
        StartBattle,
        Battle,
        Menu
    }

    public ConsoleKeyInfo keyInfo;
    public static Random rand;
    public static CustomMaths cMaths;
    public GameStates GameState { get; set;}
    public List<Seraph> PlayerTeam { get; set; }
    public string Selection { get; set; }
    public ItemStorage Inventory { get; set; }
    public BattleManager BattleHandler;
    public Menu battleMenu;
    public GameData Data { get; set; }

    public Menu CurrentMenu { get; set; }

    public GameManager()
    {
        PlayerTeam = new List<Seraph>();
        BattleHandler = new BattleManager(PlayerTeam);
        Selection = ""; 
        rand = new Random();
        Inventory = new ItemStorage();
        GameState = GameStates.Exploration;
        Data = new GameData();
    }

    public void GameLoop()
    {
        WorldMap worldMap = new WorldMap();
        worldMap.SetMap("Map01.txt");
        Display display = new Display(worldMap, this);
        display.SetWorldDisplay(worldMap.WorldMapTiles);
        display.WorldDisplay();
        display.SetPlayerPosition(15, 15);

        // Create debug inventory
        ItemStorage _inventory = new ItemStorage();
        Item blueFlower = new Item("Blue flower", "A beautifull blue flower.");
        _inventory.AddItem(blueFlower,7);
        Item blackFlower = new Item("Black flower", "I don't like this one.");
        _inventory.AddItem(blackFlower);

        Menu mainMenu = new Menu("MAIN MENU", new List<string> { "SERAPHIM", "BAG", "QUIT GAME", "CLOSE", /*"DEBUG BATTLE"*/ });
        Menu bagMenu = new Menu("BAG", _inventory);
        Menu seraphMenu = new Menu("SERAPH", PlayerTeam);

        battleMenu = new Menu("What will you do?", new List<string> { "FIGHT", "BAG", "SERAPH", "RUN" }, Menu.MenuDisplayType.battle);

        Seraph playerSeraph = Data.Summon("Lambda", 5);

        PlayerTeam.Add(playerSeraph);

        while (true)
        {
            keyInfo = Console.ReadKey();
            Selection = "";

            switch (GameState)
            {
                case GameStates.StartExploration:
                    display.WorldDisplay();
                    display.PlayerWorldDisplay(0,0);
                    GameState = GameStates.Exploration;
                    break;
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
                        case ConsoleKey.Escape:
                            GameState = GameStates.Menu;
                            CurrentMenu = mainMenu;
                            display.MenuDisplay(mainMenu);
                            break;
                    }
                    break;

                    //initiate the battle
                case GameStates.StartBattle:
                    CurrentMenu = battleMenu;
                    display.BattleDisplay(BattleHandler);
                    display.MenuDisplay(battleMenu);
                    GameState = GameStates.Battle;
                    break;


                case GameStates.Menu:
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            CurrentMenu.PreviousLine();
                            display.MenuDisplay(CurrentMenu); // Update display
                            break;
                        case ConsoleKey.DownArrow:
                            CurrentMenu.NextLine();
                            display.MenuDisplay(CurrentMenu); // Update display
                            break;
                        case ConsoleKey.LeftArrow:
                            CurrentMenu.PreviousLine();
                            display.MenuDisplay(CurrentMenu); // Update display
                            break;
                        case ConsoleKey.RightArrow:
                            CurrentMenu.NextLine();
                            display.MenuDisplay(CurrentMenu); // Update display
                            break;
                        case ConsoleKey.Enter:
                            Selection = CurrentMenu.Enter();
                            break;
                        case ConsoleKey.Escape:
                            CurrentMenu.SelectedLine = 0;
                            Selection = "CLOSE";
                            break;
                    }
                    break;
                case GameStates.Battle:
                    display.BattleDisplayUpdate();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            CurrentMenu.PreviousLine();
                            display.MenuDisplay(CurrentMenu); // Update display
                            break;
                        case ConsoleKey.DownArrow:
                            CurrentMenu.NextLine();
                            display.MenuDisplay(CurrentMenu); // Update display
                            break;
                        case ConsoleKey.Enter:
                            Selection = CurrentMenu.Enter();
                            break;
                    }
                    break;
                default:
                    break;
            }
            
            // Main Menu
            switch (Selection)
            {
                case "FIGHT":
                    CurrentMenu.SelectedLine = 0;
                    Menu abilitiesMenu = new Menu("ABILITIES", BattleHandler.CurrentPlayer._abilities);
                    CurrentMenu = abilitiesMenu;
                    display.MenuDisplay(abilitiesMenu);
                    break;
                case "BAG":
                    CurrentMenu = bagMenu;
                    if(GameState != GameStates.Battle)
                    {
                        display.WorldDisplay(); // to erase main menu
                    }
                    display.MenuDisplay(bagMenu);
                    break;
                case "SERAPH":
                    CurrentMenu = seraphMenu;
                    if (GameState != GameStates.Battle)
                    {
                        display.WorldDisplay(); // to erase main menu
                    }
                    else
                    {
                        display.BattleDisplay(BattleHandler); // to show only the normal battle
                    }
                    display.MenuDisplay(seraphMenu);
                    break;
                case "RUN":
                    GameState = GameStates.StartExploration;
                    display.WorldDisplay();
                    BattleHandler.EndBattle();
                    break;
                case "CLOSE":
                    CurrentMenu.SelectedLine = 0;
                    if(CurrentMenu == bagMenu)
                    {
                        if (GameState == GameStates.Battle)
                        {
                            CurrentMenu = battleMenu;
                            display.BattleDisplay(BattleHandler);
                            display.MenuDisplay(battleMenu);
                        }
                        else
                        {
                            CurrentMenu = mainMenu;
                            GameState = GameStates.Menu;
                            display.WorldDisplay();
                            display.MenuDisplay(mainMenu);
                        }
                    } else if(CurrentMenu == mainMenu)
                    {
                        GameState = GameStates.Exploration;
                        display.WorldDisplay();
                    } else if(CurrentMenu == battleMenu)
                    {
                        GameState = GameStates.Exploration;
                        display.WorldDisplay();
                        display.MenuDisplay(mainMenu);
                    } else if(CurrentMenu.LineType == Menu.LinesType.ability)
                    {
                        CurrentMenu = battleMenu;
                        display.BattleDisplay(BattleHandler);
                        display.MenuDisplay(battleMenu);
                    }
                    break;
                case "SAVE AND QUIT GAME":
                    // TODO
                    break;

            }
        }
    }
}
