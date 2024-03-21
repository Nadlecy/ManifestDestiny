﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Seraph
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }

    public int Level { get; private set; }
    public int _experience;
    public int _experienceReward;

    //enum StatusEffect
    //{
    //    Poison,
    //    Stun,
    //    Bleed,

    //}

    public enum Stats
    {
        hp,
        attack,
        defense,
        mana,
        magic,
        speed
    }

    public Dictionary<Stats, int> _baseStats;
    public Dictionary<Stats, int> _currentStats;
    public Dictionary<Stats, int> _statsAlterations;
    //Dictionary<StatusEffect, int> Effect;

    Dictionary<int, BattleAbility> _abilitiesUnlocks; // List of all abilities this seraph can have, and the level at which it unlocks them.
    BattleAbility[] _abilities;
    Dictionary<int, int> _xpForLevel;

    public Seraph(string name, string type, Dictionary<Stats, int> baseStats, int experienceReward, Dictionary<int,int> xpForLevel, BattleAbility[] abilities, Dictionary<int, BattleAbility> AbilitiesUnlocks, string description)
    {
        Name = name;
        Type = type;
        Description = description;

        _baseStats = baseStats;
        _currentStats = baseStats;
        _statsAlterations = new Dictionary<Stats, int>()
        {
            {Stats.attack, 0 },
            {Stats.defense, 0 },
            {Stats.magic, 0 },
            {Stats.speed, 0 },
        };
        
        _abilities = abilities;
        _abilitiesUnlocks = AbilitiesUnlocks;

        Level = 0;
        _experience = 0;
        _xpForLevel = xpForLevel;
        _experienceReward = experienceReward;
    }


    public event Action OnLevelUp;

    public int Experience
    {
        get => _experience;
        set
        {
            _experience = value;
            // Compare to see if lvl up
            if (_experience >= _xpForLevel[Level + 1])
            {
                // Level up
                OnLevelUp?.Invoke();
                Level++;
            }
        }
    }

    public void StatChange(Stats stat, int level)
    {
        _statsAlterations[stat] += level;
        if (_statsAlterations[stat] < -4) { _statsAlterations[stat] = -4; }
        else if (_statsAlterations[stat] > 4) { _statsAlterations[stat] -= 4; }

        _currentStats[stat] = (int)(_baseStats[stat] * GameManager.cMaths.StatAlterationMultiplier(this, stat));
    }


    public void HealHp(int amount)
    {
        if (amount < 0) { throw new ArgumentException("Cannot heal in the negatives.", nameof(amount)); }
        _currentStats[Stats.hp] += amount;

        //making sure hp doesnt go over the maximum
        if (_currentStats[Stats.hp] > _baseStats[Stats.hp]) { _currentStats[Stats.hp] = _baseStats[Stats.hp]; }
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0) { throw new ArgumentException("Something went wrong with the damage calculation.", nameof(amount)); }
        _currentStats[Stats.hp] -= amount;

        //making sure hp doesn't go under 0
        if (_currentStats[Stats.hp] < 0) { _currentStats[Stats.hp] = 0; }
    }
}