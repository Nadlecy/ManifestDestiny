using ManifestDestiny;
using System;
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

    public int FleeAttemps { get; set; }

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
        PlayerParticipants.Add(CurrentPlayer);
        CurrentEnemy = EnemyTeam[0];
        EnemyAILevel = AILevel;
    }

    public int EnemyAbilityChoice()
    {
        int choiceID;
        switch(EnemyAILevel)
        {
            case 1:
                // check if seraph can do something else but wait
                int mana = CurrentEnemy.CurrentStats[Seraph.Stats.mana];
                List<int> validAbilities = new List<int> { };
                for (int i = 0; i < CurrentEnemy._abilities.Count; i++)
                {
                    if (CurrentEnemy._abilities[i].Cost <= mana && CurrentEnemy._abilities[i].Name != "Wait")
                    {
                        validAbilities.Add(i);
                    }
                }
                // If validAbilities is empty, use Wait
                if(validAbilities.Count == 0)
                {
                    for (int i = 0; i < CurrentEnemy._abilities.Count; i++)
                    {
                        if (CurrentEnemy._abilities[i].Name == "Wait")
                        {
                            validAbilities.Add(i);
                        }
                    }
                }

                choiceID = validAbilities[GameManager.rand.Next(validAbilities.Count)];

                return choiceID;
            default: throw new ArgumentException();
        }
    }
    public static void Swap<T>(IList<T> list, int indexA, int indexB) // https://stackoverflow.com/questions/2094239/swap-two-items-in-listt
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    public string BattlePhase(BattleAbility playerAbility)
    {
        //if the current player seraph is slower than the enemy, enemy attacks first.
        if (CurrentPlayer.CurrentStats[Seraph.Stats.speed] < CurrentEnemy.CurrentStats[Seraph.Stats.speed])
        {
            if(BattlePhaseEnemy() == true)
            {
                return "gameOver";
                //gameover
            }
            else if(BattlePhasePlayer(playerAbility) == true)
            {
                return "win";
            }
        }
        else
        {
            if (BattlePhasePlayer(playerAbility) == true)
            {
                return "win";
            }
            else if (BattlePhaseEnemy() == true)
            {
                return "gameOver";
                //gameover
            }
        }

        CurrentEnemy.RegenMana();
        CurrentPlayer.RegenMana();

        return "";
    }

    public bool BattlePhasePlayer(BattleAbility playerAbility)
    {
        if (CurrentPlayer.CurrentStats[Seraph.Stats.mana] > playerAbility.Cost)
        {
            playerAbility.Use(CurrentPlayer, CurrentEnemy);
        }
        if (IsDead(CurrentEnemy))
        {
            if (EnemyDeath() == false)
            {
                return true;
            }
        }
        return false;
    }

    public bool BattlePhaseEnemy()
    {
        int abilityID = EnemyAbilityChoice();
        if (CurrentEnemy.CurrentStats[Seraph.Stats.mana] > CurrentEnemy._abilities[abilityID].Cost)
        {
            CurrentEnemy._abilities[abilityID].Use(CurrentEnemy, CurrentPlayer);
        }

        if (IsDead(CurrentPlayer))
        {
            if (PlayerSwitch() == false)
            {
                return true;
            }
        }
        return false;
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
            foreach (var seraph in PlayerTeam)
            {
                if (IsDead(seraph) == false)
                {
                    Swap(PlayerTeam, PlayerTeam.IndexOf(CurrentPlayer), PlayerTeam.IndexOf(seraph));
                    CurrentPlayer = seraph;
                    break;
                }
            }
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
            //seraph.Experience += CurrentEnemy._experienceReward * (CurrentEnemy.Level / CurrentPlayer.Level);
            seraph.Experience += CurrentEnemy._experienceReward;
        }

        PlayerParticipants.Clear();
    }

    public void EndBattle()
    {
        FleeAttemps = 0;
        EnemyTeam.Clear();
        PlayerParticipants.Clear();
    }
}

