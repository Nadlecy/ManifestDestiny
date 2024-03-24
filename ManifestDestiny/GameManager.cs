﻿using ManifestDestiny;
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
        Inventory
    }

    public ConsoleKeyInfo keyInfo;
    public static Random rand;
    public static CustomMaths cMaths;
    public GameStates GameState { get; set;}
    public List<Seraph> playerTeam;
    public string Selection { get; set; }
    public ItemStorage Inventory { get; set; }
    public BattleManager BattleHandler;

    public GameManager()
    {
        playerTeam = new List<Seraph>();
        BattleHandler = new BattleManager(playerTeam);
        Selection = ""; 
        rand = new Random();
        Inventory = new ItemStorage();
        GameState = GameStates.Exploration;
    }

    public void GameLoop()
    {
        WorldMap worldMap = new WorldMap();
        worldMap.SetMap("Map01.txt");
        Display display = new Display(worldMap);
        display.SetWorldDisplay(worldMap.WorldMapTiles);
        display.WorldDisplay();
        display.SetPlayerPosition(15, 15);

        // Create debug inventory
        ItemStorage _inventory = new ItemStorage();
        Item blueFlower = new Item("Blue flower", "A beatifull blue flower.");
        _inventory.AddItem(blueFlower,7);
        Item blackFlower = new Item("Black flower", "I don't like this one.");
        _inventory.AddItem(blackFlower);

        Menu mainMenu = new Menu("MAIN MENU", new List<string> { "SERAPHIM", "BAG", "QUIT GAME", "CLOSE" });
        Menu bagMenu = new Menu("BAG", _inventory.getList());

        while (true)
        {
            keyInfo = Console.ReadKey();
            Selection = "";

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
                        case ConsoleKey.Escape:
                            GameState = GameStates.Menu;
                            display.MenuDisplay(mainMenu);
                            //mainMenu.Back();
                            break;
                    }
                    break;
                case GameStates.StartBattle:
                    display.BattleDisplay(BattleHandler);
                    break;
                case GameStates.Battle:
                    
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
                case GameStates.Inventory:
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            break;
                        case ConsoleKey.RightArrow:
                            break;
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
                            //mainMenu.Back();
                            break;
                    }
                    break;

                default:
                    break;
            }
            
            // Main Menu
            switch (Selection)
            {
                case "BAG":
                    GameState = GameStates.Inventory;
                    //display.WorldDisplay();
                    display.MenuDisplay(bagMenu);
                    break;
                case "CLOSE":
                    GameState = GameStates.Exploration;
                    display.WorldDisplay();
                    break;
                case "SAVE AND QUIT GAME":
                    // TODO
                    break;
            }
            // MOVE THIS TO MENU CLASS
        }
    }
}
