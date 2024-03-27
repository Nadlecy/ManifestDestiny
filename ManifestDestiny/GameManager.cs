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
        Menu,
        Inventory,
        SeraphMenu
    }

    public ConsoleKeyInfo keyInfo;
    public static Random rand;
    public static CustomMaths cMaths;
    public GameStates GameState { get; set;}
    public List<Seraph> PlayerTeam { get; set; }
    public string Selection { get; set; }
    public ItemStorage Inventory { get; set; }
    public bool InBattle { get; set; }
    public BattleManager BattleHandler;
    public Menu battleMenu;
    public GameData Data { get; set; }

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

        Menu mainMenu = new Menu("MAIN MENU", new List<string> { "SERAPHIM", "BAG", "QUIT GAME", "CLOSE", "DEBUG BATTLE" });
        Menu bagMenu = new Menu("BAG", _inventory);
        Menu seraphMenu = new Menu("SERAPH", PlayerTeam);

        battleMenu = new Menu("What will you do?", new List<string> { "FIGHT", "BAG", "SERAPH", "RUN" });

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
                            display.MenuDisplay(mainMenu);
                            break;
                    }
                    break;

                    //initiate the battle
                case GameStates.StartBattle:
                    display.BattleDisplay(BattleHandler);
                    GameState = GameStates.Battle;
                    break;


                case GameStates.Menu:
                    switch (keyInfo.Key)
                    {
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
                            Selection = "CLOSE";
                            break;
                    }
                    break;
                case GameStates.Inventory:
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            bagMenu.PreviousLine();
                            display.MenuDisplay(bagMenu); // Update display
                            break;
                        case ConsoleKey.DownArrow:
                            bagMenu.NextLine();
                            display.MenuDisplay(bagMenu); // Update display
                            break;
                        case ConsoleKey.Enter:
                            Selection = bagMenu.Enter();
                            break;
                        case ConsoleKey.Escape:
                            bagMenu.SelectedLine = 0;
                            Selection = "CLOSE";
                            //mainMenu.Back();
                            break;
                    }
                    break;
                case GameStates.Battle:
                    display.BattleDisplayUpdate();
                    InBattle = true;
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            battleMenu.PreviousLine();
                            display.MenuDisplay(battleMenu, Display.MenuDisplayType.battle); // Update display
                            break;
                        case ConsoleKey.DownArrow:
                            battleMenu.NextLine();
                            display.MenuDisplay(battleMenu, Display.MenuDisplayType.battle); // Update display
                            break;
                        case ConsoleKey.Enter:
                            Selection = battleMenu.Enter();
                            break;
                    }
                    break;
                default:
                    break;
            }
            
            // Main Menu
            switch (Selection)
            {
                case "DEBUG BATTLE":
                    mainMenu.SelectedLine = 0;
                    GameState = GameStates.StartBattle;
                    display.BattleDisplay(BattleHandler);
                    display.MenuDisplay(battleMenu, Display.MenuDisplayType.battle);
                    break;
                case "FIGHT":
                    battleMenu.SelectedLine = 0;
                    Menu abilitiesMenu = new Menu("ABILITIES", BattleHandler.CurrentPlayer._abilities);
                    display.MenuDisplay(abilitiesMenu, Display.MenuDisplayType.abilities);
                    break;
                case "BAG":
                    GameState = GameStates.Inventory;
                    if(InBattle == false)
                    {
                        display.WorldDisplay(); // to erase main menu
                    }
                    display.MenuDisplay(bagMenu);
                    break;
                case "SERAPH":
                    GameState = GameStates.SeraphMenu;
                    if (InBattle == false)
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
                    InBattle = false;
                    battleMenu.SelectedLine = 0;
                    GameState = GameStates.StartExploration;
                    display.WorldDisplay();
                    display.MenuDisplay(mainMenu);
                    BattleHandler.EndBattle();
                    break;
                case "CLOSE":
                    switch (GameState)
                    {
                        case GameStates.Inventory:
                            if (InBattle)
                            {
                                GameState = GameStates.Battle;
                                display.BattleDisplay(BattleHandler);
                                display.MenuDisplay(battleMenu, Display.MenuDisplayType.battle);
                            } else
                            {
                                GameState = GameStates.Menu;
                                display.WorldDisplay();
                                display.MenuDisplay(mainMenu);
                            }
                            bagMenu.SelectedLine = 0;
                            
                            break;
                        case GameStates.Menu:
                            mainMenu.SelectedLine = 0;
                            GameState = GameStates.Exploration;
                            display.WorldDisplay();
                            break;
                        case GameStates.Battle:
                            battleMenu.SelectedLine = 0;
                            GameState = GameStates.Exploration;
                            display.WorldDisplay();
                            display.MenuDisplay(mainMenu);
                            break;
                    }
                    break;
                case "SAVE AND QUIT GAME":
                    // TODO
                    break;

            }
        }
    }
}
