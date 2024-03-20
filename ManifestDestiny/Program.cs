
// See https://aka.ms/new-console-template for more information
using ManifestDestiny;
class Program
{

    static void Main(string[] args)
    {
        //a few setting changes before we start
        Console.CursorVisible = false;

        //entering the code proper
        GameManager gameManager = new GameManager();
        gameManager.GameLoop();

        
    }
}