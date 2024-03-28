using ManifestDestiny;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
    WorldMap MapWorld { get; set;}
    public Menu CurrentMenu { get; set; }
    public Save Saving { get; set; }
    Position _playerPosition;

    string _map;
    bool _isGettingKey = true;

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

    public void DetectPlayer()
    {
        if (MapWorld.WorldMapTiles[PlayerPosition.X][PlayerPosition.Y].IsHealer == true)
        {
            foreach (var seraph in PlayerTeam)
            {
                seraph.FullHeal();
            }
        }
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
        Inventory.AddItem(Items.ItemList["Basic Xenoconverter"].Clone(), 1000);

        Menu mainMenu = new Menu("MAIN MENU", new List<string> { "SERAPHIM", "BAG", "SAVE AND QUIT GAME", "CLOSE"});
        Menu bagMenu = new Menu("BAG", Inventory);
        Menu seraphMenu = new Menu("SERAPHIM", PlayerTeam);

        battleMenu = new Menu("What will you do?", new List<string> { "FIGHT", "BAG", "SERAPHIM", "RUN" }, Menu.MenuDisplayType.battle);

        bool justLeftBubbles;

        //PlayerTeam.Add(ju);
        //PlayerTeam.Add(gagaga);

        //save.TeamJsonWriter("SaveSeraph", PlayerTeam);

        MapWorld = new WorldMap(this);
        MapWorld.SetMap(Map);
        Display display = new Display(MapWorld, this);
        display.SetWorldDisplay(MapWorld.WorldMapTiles);
        display.WorldDisplay();
        display.SetPlayerPosition(PlayerPosition.X, PlayerPosition.Y);

        while (Gaming)
        {
            justLeftBubbles = false;
            if (_isGettingKey == true)
            {
                keyInfo = Console.ReadKey(true);
            }


            if (DialogBubbles.Count > 0)
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    DialogBubbles.RemoveAt(0);
                    justLeftBubbles = true;
                    if(GameState == GameStates.StartExploration || GameState == GameStates.Exploration)
                    {
                        display.WorldDisplay();
                        display.PlayerWorldDisplay(0, 0);
                    } else if(GameState == GameStates.Menu || GameState == GameStates.Battle)
                    {
                        display.MenuDisplay(CurrentMenu);
                    }
                }
            }

            if (DialogBubbles.Count == 0)
            {
                Selection = "";
                switch (GameState)
                {
                    case GameStates.StartExploration:
                        //DialogBubbles.Add("Welcome to exploration mode");
                        _isGettingKey = true;
                        display.WorldDisplay();
                        display.PlayerWorldDisplay(0, 0);
                        GameState = GameStates.Exploration;
                        break;
                    case GameStates.Exploration:
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                display.PlayerWorldDisplay(0, -1);
                                DetectPlayer();
                                break;
                            case ConsoleKey.RightArrow:
                                display.PlayerWorldDisplay(0, 1);
                                DetectPlayer();
                                break;
                            case ConsoleKey.UpArrow:
                                display.PlayerWorldDisplay(-1, 0);
                                DetectPlayer();
                                break;
                            case ConsoleKey.DownArrow:
                                display.PlayerWorldDisplay(1, 0);
                                DetectPlayer();
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
                        DialogBubbles.Add("A wild " + BattleHandler.CurrentEnemy.Name + " appears !");
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
                                if (justLeftBubbles == false)
                                {
                                    Selection = CurrentMenu.Enter(BattleHandler);
                                }
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
                                if (justLeftBubbles == false)
                                {
                                    Selection = CurrentMenu.Enter(BattleHandler);

                                    // Item selection
                                    if (CurrentMenu.LineType == Menu.LinesType.items && Selection != "CLOSE")
                                    {
                                        // Do item thing
                                        CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].Use();

                                        if (CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].Heal > 0)
                                        {
                                            // Heal seraph
                                            BattleHandler.CurrentPlayer.HealHp(CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].Heal);
                                            CurrentMenu.ItemStorage.RemoveItem(CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine]);
                                            Selection = "CLOSE";
                                        }
                                        else if (CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine].CatchRateMultiplier > 0)
                                        {
                                            // Try to catch enemy
                                            float catchRate = cMaths.CatchRateCalculator(BattleHandler.CurrentEnemy, CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine]);
                                            double catchRand = rand.NextDouble();
                                            if (catchRate > catchRand)
                                            {
                                                DialogBubbles.Add("You catched " + BattleHandler.CurrentEnemy.Name);
                                                PlayerTeam.Add(BattleHandler.CurrentEnemy);
                                                BattleHandler.EndBattle();
                                                GameState = GameStates.StartExploration;
                                                _isGettingKey = false;
                                            }
                                            else
                                            {
                                                DialogBubbles.Add("You missed " + BattleHandler.CurrentEnemy.Name);
                                            }
                                            CurrentMenu.ItemStorage.RemoveItem(CurrentMenu.ItemStorage.Items[CurrentMenu.SelectedLine]);

                                            //Selection = "CLOSE";
                                        }
                                    }

                                    // Attack selection
                                    if (CurrentMenu.LineType == Menu.LinesType.ability && Selection != "CLOSE")
                                    {
                                        string turnResult = BattleHandler.BattlePhase(BattleHandler.CurrentPlayer._abilities[CurrentMenu.SelectedLine]);
                                        Selection = "CLOSE";
                                        if (turnResult == "gameOver")
                                        {
                                            DialogBubbles.Add("Game Over");
                                            BattleHandler.EndBattle();
                                            //switch to a GameOver Gamestate or something idk
                                            LoadSave();
                                        }
                                        else if (turnResult == "win")
                                        {
                                            DialogBubbles.Add("You won the battle");
                                            BattleHandler.EndBattle();
                                            _isGettingKey = false;
                                            GameState = GameStates.StartExploration;
                                        }
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
                        if (GameState != GameStates.Battle)
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
                        if (flee > 255)
                        {
                            sucess = true;
                        }
                        else if (rand.Next(256) <= flee)
                        {
                            sucess = true;
                        }

                        if (sucess)
                        {
                            DialogBubbles.Add(BattleHandler.CurrentPlayer.Name + " escaped the battle");
                            GameState = GameStates.StartExploration;
                            display.WorldDisplay();
                            BattleHandler.EndBattle();
                        }
                        else
                        {
                            DialogBubbles.Add(BattleHandler.CurrentPlayer.Name + " wasn't able to escape!");
                            // skip your turn
                            BattleHandler.BattlePhaseEnemy();
                            display.BattleDisplay(BattleHandler);
                            display.MenuDisplay(battleMenu);
                            Console.WriteLine("Fleeing failed.");
                        }

                        break;
                    case "CLOSE":
                        if (CurrentMenu.LineType != Menu.LinesType.ability) { CurrentMenu.SelectedLine = 0; }

                        if (CurrentMenu == bagMenu)
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
                        else if (CurrentMenu == mainMenu)
                        {
                            _isGettingKey = false;
                            GameState = GameStates.StartExploration;
                            display.WorldDisplay();
                        }
                        else if (CurrentMenu == battleMenu)
                        {
                            GameState = GameStates.StartExploration;
                            display.WorldDisplay();
                            display.MenuDisplay(mainMenu);
                        }
                        else if (CurrentMenu.LineType == Menu.LinesType.ability)
                        {
                            CurrentMenu = battleMenu;
                            display.BattleDisplay(BattleHandler);
                            display.MenuDisplay(battleMenu);
                        }
                        else if (CurrentMenu == seraphMenu)
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
                        //BattleHandler.CurrentPlayer;
                        display.BattleDisplay(BattleHandler);
                        display.MenuDisplay(seraphMenu);
                        break;
                    case "SAVE AND QUIT GAME":
                        Gaming = false;
                        // TODO
                        break;
                }
            }

            if (DialogBubbles.Count > 0)
            {
                display.BubbleDisplay(DialogBubbles);
            }
        }
        //Fin du jeu
        WriteSave();
    }
}
