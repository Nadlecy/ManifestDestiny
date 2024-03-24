
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

        int width = 150; // Largeur désirée en caractères
        int height = 50; // Hauteur désirée en lignes
        Console.SetWindowSize(width, height);

        GameManager gameManager = new GameManager();
        
        gameManager.GameLoop();
    }
}