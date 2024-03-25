
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
using System.Text.Json;
class Program
{

    static void Main(string[] args)
    {
        //a few setting changes before we start
        //Console.CursorVisible = false;

        //entering the code proper



        // capacités de test
        List<Tuple<BattleAbility, List<AbilityAttribute>>> abilities = new();

        GameData gameData = new GameData();

        Seraph test = gameData.Summon("Lambda", 3);
        Seraph testest = gameData.Summon("Lambda", 2);

        GameManager gameManager = new GameManager();

        gameManager.playerTeam.Add(test);

        gameManager.BattleHandler.StartBattle(new List<Seraph> { testest });
        /*
        Console.WriteLine("player hp : " + gameManager.BattleHandler.CurrentPlayer._currentStats[Seraph.Stats.hp]);
        Console.WriteLine("enemy hp : " + gameManager.BattleHandler.CurrentEnemy._currentStats[Seraph.Stats.hp]);

        gameManager.BattleHandler.BattlePhase(gameManager.BattleHandler.CurrentPlayer._abilities[0], gameManager.BattleHandler.CurrentEnemy._abilities[0]);

        Console.WriteLine("player hp : " + gameManager.BattleHandler.CurrentPlayer._currentStats[Seraph.Stats.hp]);
        Console.WriteLine("enemy hp : " + gameManager.BattleHandler.CurrentEnemy._currentStats[Seraph.Stats.hp]);
        */

        
        int width = 150; // Largeur désirée en caractères
        int height = 50; // Hauteur désirée en lignes
        Console.SetWindowSize(width, height);

        
        gameManager.GameLoop();
        
    }
}