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
        power,
        resistance,
        mana,
        magic,
        speed
    }

    Dictionary<Stats, int> _baseStats;
    Dictionary<Stats, int> _currentStats;
    Dictionary<Stats, int> _statsAlterations;
    //Dictionary<StatusEffect, int> Effect;

    Dictionary<int, Ability> _abilitiesUnlocks; // List of all abilities this seraph can have, and the level at which it unlocks them.
    Ability[] _abilities;
    Dictionary<int, int> _xpForLevel;

    public Seraph(string name, string type, Dictionary<Stats, int> baseStats, Dictionary<int,int> xpForLevel, Ability[] abilities, Dictionary<int, Ability> AbilitiesUnlocks, string description)
    {
        Name = name;
        Type = type;
        Description = description;

        _baseStats = baseStats;
        _currentStats = baseStats;
        _statsAlterations = new Dictionary<Stats, int>()
        {
            {Stats.power, 0 },
            {Stats.resistance, 0 },
            {Stats.mana, 0 },
            {Stats.magic, 0 },
            {Stats.speed, 0 },
        };
        
        _abilities = abilities;
        _abilitiesUnlocks = AbilitiesUnlocks;

        Level = 0;
        _experience = 0;
        _xpForLevel = xpForLevel;
    }

    public int Experience {
        get => _experience;
        set { 
            _experience = value;
            // Compare to see if lvl up
        }
    }
}