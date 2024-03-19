﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Ability
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int Accuracy { get; set; }
    public int CritChance { get; set; }
    public int Power { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }

    public Ability(string name, string type, int accuracy, int critChance, int power, int cost, string description)
    {
        Name = name;
        Type = type;
        Accuracy = accuracy;
        CritChance = critChance;
        Power = power;
        Cost = cost;
        Description = description;
    }

    public void Use() {
        // Apply damage or custom code
    }
}
