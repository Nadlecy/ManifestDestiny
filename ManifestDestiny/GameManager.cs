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
    }
    
    ConsoleKeyInfo keyInfo;
    Random rand;

    public GameManager()
    {
        GameStates gameState = GameStates.Exploration;
    }
}
