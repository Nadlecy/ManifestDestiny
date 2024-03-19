
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{
    public static Random rnd = new Random();

    static void Main(string[] args)
    {
        GameManager gameManager = new GameManager();

        WorldMap worldMap = new WorldMap();
        worldMap.Init();
        worldMap.SetMap("Map01.txt");
        Display display = new Display();

        while (true)
        {
            display.SetDisplay(worldMap.WorldMapTiles);


            display.ShowDisplay();
        }
    }
}