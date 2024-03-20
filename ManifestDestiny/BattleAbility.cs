using ManifestDestiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

class BattleAbility
{
    public string Name { get; set; }
    public BattleType BattleType{ get; set; }
    public int Accuracy { get; set; }
    public int CritChance { get; set; }
    public int Power { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }

    public BattleAbility(string name, BattleType type, int accuracy, int critChance, int power, int cost, string description)
    {
        Name = name;
        BattleType = type;
        Accuracy = accuracy;
        CritChance = critChance;
        Power = power;
        Cost = cost;
        Description = description;
    }

    public virtual void Use(Seraph caster, Seraph target) {
        // Apply damage or custom code
        if (Power != 0)
        {
            int dmg = GameManager.cMaths.DamageCalculator(caster, target, this);
            target._currentStats[Seraph.Stats.hp] -= dmg;
        }

        caster._currentStats[Seraph.Stats.mana] -= Cost;
    }
}
