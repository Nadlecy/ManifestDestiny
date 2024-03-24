using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleManager
{
    public List<Seraph> enemyTeam;
    public List<Seraph> playerTeam;
    public Seraph CurrentPlayer { get; private set; }
    public Seraph CurrentEnemy { get; private set; }

    public BattleManager(List<Seraph> playerList)
    {
        playerTeam = playerList;
        enemyTeam = new List<Seraph>();

    }

    public void StartBattle(List<Seraph>foeTeam)
    {
        if (foeTeam.Count > 6) { throw new ArgumentException("too many ennemies", nameof(foeTeam)); }
        enemyTeam = foeTeam;
        CurrentPlayer = playerTeam[0];
        CurrentEnemy = enemyTeam[0];
    }

    public void PlayerChoice()
    {

    }

    public bool Escape()
    {

        return true;
    }

    public void EndBattle()
    {
        enemyTeam.Clear();
        
    }
}

