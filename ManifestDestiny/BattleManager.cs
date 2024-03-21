using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleManager
{
    public List<Seraph> enemyTeam;
    public GameManager GameManager {  get; private set; }
    public Seraph CurrentPlayer { get; private set; }
    public Seraph CurrentEnemy { get; private set; }

    public BattleManager(GameManager manager)
    {
        GameManager = manager;
        enemyTeam = new List<Seraph>();

    }

    public void StartBattle(List<Seraph>foeTeam)
    {
        if (foeTeam.Count > 6) { throw new ArgumentException("too many ennemies", nameof(foeTeam)); }
        enemyTeam = foeTeam;
        CurrentPlayer = GameManager.playerTeam[0];
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

