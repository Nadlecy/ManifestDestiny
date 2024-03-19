using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Ability
{
    public string _name;
    public string _type;
    public int _accuracy;
    public int _critChance;
    public int _power;
    public int _cost;
    public string _description;

    public Dictionary<string, float> _typeRelations = new Dictionary<string, float>();
    public string _statusEffectImmunity;

    public Ability(string name, string type, int accuracy, int critChance, int power, int cost, string description)
    {
        _name = name;
        _type = type;
        _accuracy = accuracy;
        _critChance = critChance;
        _power = power;
        _cost = cost;
        _description = description;
    }

    public void Use() {
        // Apply damage or custom code
    }
}
