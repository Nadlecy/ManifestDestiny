﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleManager
{
    public List<Seraph> EnemyTeam { get; set; } 
    public List<Seraph> PlayerTeam { get; set; }
    public List<Seraph> PlayerParticipants { get; set; }
    public int EnemyAILevel { get; set; } 
    public Seraph CurrentPlayer { get; set; }
    public Seraph CurrentEnemy { get; private set; }

    public BattleManager(List<Seraph> playerList)
    {
        PlayerTeam = playerList;
        EnemyTeam = new List<Seraph>();
        EnemyAILevel = 1;
        PlayerParticipants = new List<Seraph>();
    }

    public void StartBattle(List<Seraph>foeTeam, int AILevel)
    {
        if (foeTeam.Count > 6) { throw new ArgumentException("too many ennemies", nameof(foeTeam)); }
        EnemyTeam = foeTeam;
        CurrentPlayer = PlayerTeam[0];
        CurrentEnemy = EnemyTeam[0];
        EnemyAILevel = AILevel;
    }

    public void BattlePhase(BattleAbility playerAbility, BattleAbility enemyAbility)
    {
        //if the current player seraph is slower than the enemy, enemy attacks first.
        if (CurrentPlayer.CurrentStats[Seraph.Stats.speed] < CurrentEnemy.CurrentStats[Seraph.Stats.speed])
        {
            if(BattlePhaseEnemy(enemyAbility) == false)
            {

            }else if(BattlePhasePlayer(playerAbility) == false)
            {

            }
        }
        else
        {
            if (BattlePhasePlayer(playerAbility) == false)
            {

            }
            else if (BattlePhaseEnemy(enemyAbility) == false)
            {

            }
        }
    }

    public bool BattlePhasePlayer(BattleAbility playerAbility)
    {
        playerAbility.Use(CurrentPlayer, CurrentEnemy);
        if (IsDead(CurrentEnemy))
        {
            if (EnemyDeath() == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool BattlePhaseEnemy(BattleAbility enemyAbility)
    {
        enemyAbility.Use(CurrentEnemy, CurrentPlayer);
        if (IsDead(CurrentPlayer))
        {
            if (PlayerSwitch() == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsDead(Seraph seraph)
    {
        if (seraph.CurrentStats[Seraph.Stats.hp] == 0)
        {
            return true;
        }else return false;
    }

    public bool PlayerSwitch()
    {
        List<Seraph> alive = new();
        //check if player can switch
        foreach (Seraph seraph in PlayerTeam)
        {
            if (IsDead(seraph) == false)
            {
                alive.Add(seraph);
            }
        }
        if (alive.Count > 0)
        {
            //force player to select new seraph to send out, in a menu
            /*
             * MENU THING HERE
             */

            //if the new seraph has not fought before, add it to the list of participants
            if (PlayerParticipants.Contains(CurrentPlayer) == false)
            {
                PlayerParticipants.Add(CurrentPlayer);
            }
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool EnemyDeath()
    {
        GetEnemyExp();

        foreach (Seraph seraph in EnemyTeam)
        {
            if (seraph.CurrentStats[Seraph.Stats.hp] > 0)
            {
                CurrentEnemy = seraph;
                return true;
            }
        }
        return false;
    }

    public void GetEnemyExp()
    {
        foreach (Seraph seraph in PlayerParticipants)
        {
            seraph.Experience += CurrentEnemy._experienceReward * (CurrentEnemy.Level / CurrentPlayer.Level);
        }

        PlayerParticipants.Clear();
    }

    public void EndBattle()
    {
        EnemyTeam.Clear();
        PlayerParticipants.Clear();
    }
}

