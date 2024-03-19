
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        WorldMap worldMap = new WorldMap();
        worldMap.Init();
        worldMap.CreateMap("Map01.txt");
        Display display = new Display();
        display.SetDiplay(worldMap.WorldMapTiles);
        display.ShowDisplay();
    }
}
