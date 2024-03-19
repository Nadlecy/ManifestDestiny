
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{
    static void Main(string[] args)
    {
        GameManager gameManager = new GameManager();

        WorldMap worldMap = new WorldMap();
        worldMap.Init();
        worldMap.CreateMap("Map01.txt");
        Display display = new Display();

        while (true)
        {
            display.SetDisplay(worldMap.WorldMapTiles);


            display.ShowDisplay();
        }
    }
}
