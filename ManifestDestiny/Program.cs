
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{

    static void Main(string[] args)
    {
        //a few setting changes before we start
        //Console.CursorVisible = false;
        
        //entering the code proper

        int width = 150; // Largeur désirée en caractères
        int height = 50; // Hauteur désirée en lignes
        Console.SetWindowSize(width, height);

        GameManager gameManager = new GameManager();
        gameManager.GameLoop();
    }
}