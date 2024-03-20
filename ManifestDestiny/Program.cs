
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{

    static void Main(string[] args)
    {
        //a few setting changes before we start
        //Console.CursorVisible = false;

        //entering the code proper
        GameManager gameManager = new GameManager();

        WorldMap worldMap = new WorldMap();
        worldMap.Init();
        worldMap.SetMap("Map01.txt");
        Display display = new Display();

        while (true)
        {
            gameManager.keyInfo = Console.ReadKey();


            display.SetWorldDisplay(worldMap.WorldMapTiles);
            display.WorldDisplay();
        }
    }
}