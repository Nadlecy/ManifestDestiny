using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameManager
{
    enum GameStates {
        Exploration,
        Battle,
        Menu,
    }
    
    public ConsoleKeyInfo keyInfo;
    public static Random rand;

    public GameManager()
    {
        rand = new Random();
        GameStates gameState = GameStates.Exploration;
    }
}
