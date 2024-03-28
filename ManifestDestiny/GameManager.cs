using ManifestDestiny;
using ManifestDestiny.Container;
using ManifestDestiny.View;
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
    public static List<string> DialogBubbles {  get; set; }

    public GameStates GameState { get; set;}
    public List<Seraph> PlayerTeam { get; set; }
    public string Selection { get; set; }
    public ItemStorage Inventory { get; set; }
    public ItemContainer Items { get; set; }
    public bool InBattle { get; set; }
    public BattleManager BattleHandler;
    public Menu battleMenu;
    public GameData Data { get; set; }
    public bool Gaming { get; set; }

    public Menu CurrentMenu { get; set; }
    public Save Saving { get; set; }
    Position _playerPosition;

    string _map;

    public string Map { get => _map; set => _map = value; }
    public Position PlayerPosition { get => _playerPosition; set => _playerPosition = value; }

    public GameManager()
    {
        DialogBubbles = new List<string>();

        Saving = new Save();
        Gaming = true;
        PlayerTeam = new List<Seraph>();
        BattleHandler = new BattleManager(PlayerTeam);
        Selection = ""; 
        rand = new Random();
        Inventory = new ItemStorage();
        GameState = GameStates.Exploration;
        Data = new GameData();
    }

    public void LoadSave()
    {
        PlayerTeam = Saving.TeamJsonLoader("SaveSeraph", PlayerTeam, Data);
        Saving.PositionJsonLoader("SavePosition", ref _playerPosition, ref _map);
    }

    public void WriteSave()
    {
        Saving.TeamJsonWriter("SaveSeraph", PlayerTeam);
        Saving.PositionJsonWriter("SavePosition", PlayerPosition, ref _map);
    }

    public void GameLoop()
    {
        LoadSave();


        // Create debug inventory

        // Créer une instance de JsonReader pour désérialiser une liste de warps
        CustomJson<ItemContainer> jsonReader = new CustomJson<ItemContainer>("Item/Items.json");

        // Lire les données JSON et obtenir la liste de warps
        Items = jsonReader.Read();


        //_inventory.AddItem(Items.ItemList["Tasty Ration"].Clone(),7);
        Inventory.AddItem(Items.ItemList["Black Flower"].Clone());

        Menu mainMenu = new Menu("MAIN MENU", new List<string> { "SERAPHIM", "BAG", "SAVE AND QUIT GAME", "CLOSE"});
        Menu bagMenu = new Menu("BAG", Inventory);
        Menu seraphMenu = new Menu("SERAPHIM", PlayerTeam);

        battleMenu = new Menu("What will you do?", new List<string> { "FIGHT", "BAG", "SERAPHIM", "RUN" }, Menu.MenuDisplayType.battle);

        Seraph ju = Data.Summon("Lambda", 5);
        Seraph gagaga = Data.Summon("Bit", 32);

        //PlayerTeam.Add(ju);
        //PlayerTeam.Add(gagaga);

        //save.TeamJsonWriter("SaveSeraph", PlayerTeam);

        WorldMap worldMap = new WorldMap(this);
        worldMap.SetMap(Map);
        Display display = new Display(worldMap, this);
        display.SetWorldDisplay(worldMap.WorldMapTiles);
        display.WorldDisplay();
        display.SetPlayerPosition(PlayerPosition.X, PlayerPosition.Y);

        while (Gaming)
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
                            //CurrentMenu.SelectedLine = 0;
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

                            // Item selection
                            if(CurrentMenu.LineType == Menu.LinesType.items && Selection != "CLOSE")
                            {
                                // Do item thing
                                CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].Use();
                                if (CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].Heal > 0)
                                {
                                    // Heal seraph
                                    BattleHandler.CurrentPlayer.HealHp(CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].Heal);
                                    CurrentMenu.ItemStorage.Items.RemoveAt(CurrentMenu.SelectedLine);
                                    Selection = "CLOSE";
                                }
                            }

                            // Attack selection
                            if (CurrentMenu.LineType == Menu.LinesType.ability && Selection != "CLOSE")
                            {
                                string turnResult = BattleHandler.BattlePhase(BattleHandler.CurrentPlayer._abilities[CurrentMenu.SelectedLine]);
                                Selection = "CLOSE";
                                if(turnResult == "gameOver")
                                {
                                    BattleHandler.EndBattle();
                                    //switch to a GameOver Gamestate or something idk

                                }else if(turnResult == "win")
                                {
                                    BattleHandler.EndBattle();
                                    GameState = GameStates.StartExploration;
                                }
                            }
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
                case "SERAPHIM":
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
                    bool sucess = false;
                    // Calcul
                    BattleHandler.FleeAttemps++;
                    int flee = (BattleHandler.CurrentPlayer.CurrentStats[Seraph.Stats.speed] * 32) / BattleHandler.CurrentEnemy.CurrentStats[Seraph.Stats.speed] + 30 * BattleHandler.FleeAttemps;
                    if(flee > 255)
                    {
                        sucess = true;   
                    } else if(rand.Next(256) <= flee)
                    {
                        sucess=true;
                    }

                    if (sucess)
                    {
                        GameState = GameStates.StartExploration;
                        display.WorldDisplay();
                        BattleHandler.EndBattle();
                    } else
                    {
                        
                        // skip your turn
                        BattleHandler.BattlePhaseEnemy();
                        display.BattleDisplay(BattleHandler);
                        display.MenuDisplay(battleMenu);
                        Console.WriteLine("Fleeing failed.");
                    }
                    
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
                        GameState = GameStates.StartExploration;
                        display.WorldDisplay();
                    } else if(CurrentMenu == battleMenu)
                    {
                        GameState = GameStates.StartExploration;
                        display.WorldDisplay();
                        display.MenuDisplay(mainMenu);
                    } else if(CurrentMenu.LineType == Menu.LinesType.ability)
                    {
                        CurrentMenu = battleMenu;
                        display.BattleDisplay(BattleHandler);
                        display.MenuDisplay(battleMenu);
                    } else if(CurrentMenu == seraphMenu)
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
                    }
                    break;
                case "Switched two seraph":
                    // update seraphim menu display
                    display.MenuDisplay(seraphMenu);
                    break;
                case "SAVE AND QUIT GAME":
                    Gaming = false;
                    // TODO
                    break;

            }
        }
        //Fin du jeu
        WriteSave();
    }
}
