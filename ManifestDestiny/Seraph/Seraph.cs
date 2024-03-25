using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class Seraph
{
    public string Name { get; set; }
    public BattleType Type { get; set; }
    public string Description { get; set; }

    public int Level { get; set; }
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
    public Dictionary<Stats, int> _maxStats;
    public Dictionary<Stats, int> _statsAlterations;
    //Dictionary<StatusEffect, int> Effect;

    private Dictionary<int, BattleAbility> _abilitiesUnlocks; // List of all abilities this seraph can have, and the level at which it unlocks them.
    public List<BattleAbility> _abilities;

    public Seraph(string name, BattleType type, Dictionary<Stats, int> baseStats, Dictionary<Stats, int> maxStats, int experienceReward, Dictionary<int, BattleAbility> AbilitiesUnlocks, string description)
    {
        Name = name;
        Type = type;
        Description = description;

        _baseStats = baseStats;
        _currentStats = baseStats;
        _maxStats = maxStats;
        _statsAlterations = new Dictionary<Stats, int>()
        {
            {Stats.attack, 0 },
            {Stats.defense, 0 },
            {Stats.magic, 0 },
            {Stats.speed, 0 },
        };
        
        _abilities = new();
        _abilitiesUnlocks = AbilitiesUnlocks;

        Level = 0;
        _experience = 0;
        _experienceReward = experienceReward;
    }


    public event Action OnLevelUp;

    public void GainSkill()
    {
        if (_abilitiesUnlocks.ContainsKey(Level) && _abilities.Contains(_abilitiesUnlocks[Level]) == false)
        {
            _abilities.Add(_abilitiesUnlocks[Level]);
        }
    }

    public int Experience
    {
        get => _experience;
        set
        {
            _experience = value;
            // Compare to see if lvl up
            while (_experience >= 100)
            {
                _experience -= 100;
                // Level up
                foreach (Stats currentStat in Enum.GetValues(typeof(Stats)))
                {
                    _baseStats[currentStat] = (_maxStats[currentStat] - _baseStats[currentStat]) / (100 - Level);
                }

                OnLevelUp?.Invoke();
                Level++;
                GainSkill();
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

    public void StatReset()
    {
        foreach (Stats key in _statsAlterations.Keys)
        {
            _statsAlterations[key] = 0;
        }
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

    //performs a deep copy of a seraph
    public Seraph Clone()
    {
        Seraph copy = (Seraph) MemberwiseClone();
        //these two do not change so we can refer to the same instance
        copy._maxStats = _maxStats;
        copy._abilitiesUnlocks = _abilitiesUnlocks;

        //these four need to be their own instances, however
        copy._baseStats = new Dictionary<Stats, int>(_baseStats);
        copy._currentStats = new Dictionary<Stats, int>(_currentStats);
        copy._statsAlterations = new Dictionary<Stats, int>(_statsAlterations);
        copy._abilities = new();

        return copy;
    }
}