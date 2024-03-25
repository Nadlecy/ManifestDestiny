using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleManager
{
    public List<Seraph> enemyTeam;
    public List<Seraph> playerTeam;
    public Seraph CurrentPlayer { get; set; }
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

    public void BattlePhase(BattleAbility playerAbility, BattleAbility enemyAbility)
    {
        //check for priority/speed advantage

        //if the current player seraph is slower than the enemy, enemy attacks first.
        if (CurrentPlayer._currentStats[Seraph.Stats.speed] < CurrentEnemy._currentStats[Seraph.Stats.speed])
        {
            enemyAbility.Use(CurrentEnemy, CurrentPlayer);
            //death check + player switch-in/gameover
            playerAbility.Use(CurrentPlayer, CurrentEnemy);
            //death check + enemy switch-in/battle end
        }
        else
        {
            playerAbility.Use(CurrentPlayer, CurrentEnemy);
            //death check + enemy switch-in/battle end
            enemyAbility.Use(CurrentEnemy, CurrentPlayer);
            //death check + player switch-in/gameover
        }

    }

    public bool DeathCheck(Seraph seraph)
    {
        if (seraph._currentStats[Seraph.Stats.hp] == 0)
        {
            return true;
        }else return false;
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

