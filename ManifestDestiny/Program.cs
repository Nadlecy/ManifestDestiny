
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
        /*
        List<BattleAbility> abilities = new();

        BattleType Absolute = new("Absolute");
        BattleAbility test = new("testAbility",Absolute, 90, 12, "A test ability, lowers enemy defense");
        test.AddAttribute(new AbilityAttributeAttack(10, 40, test.BattleType));
        test.AddAttribute(new AbilityAttributeStatAlteration(new List<int> { 0, 0, 0, 0, -1, 0 }));
        abilities.Add(test);

        //on serialize tout ça
        string fileName = "../../../Data/Attacks.json";
        string jsonString = JsonSerializer.Serialize(abilities);
        File.WriteAllText(fileName, jsonString);

        */
        
        int width = 150; // Largeur désirée en caractères
        int height = 50; // Hauteur désirée en lignes
        Console.SetWindowSize(width, height);

        GameManager gameManager = new GameManager();
        
        gameManager.GameLoop();
    }
}