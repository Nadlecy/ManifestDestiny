using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

class Ability
{
    public string Name { get; set; }
    public BattleType battleType{ get; set; }
    public int Accuracy { get; set; }
    public int CritChance { get; set; }
    public int Power { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }

    public Ability(string name, BattleType type, int accuracy, int critChance, int power, int cost, string description)
    {
        Name = name;
        battleType = type;
        Accuracy = accuracy;
        CritChance = critChance;
        Power = power;
        Cost = cost;
        Description = description;
    }

    public void Use(Seraph caster, Seraph target) {
        // Apply damage or custom code
        int Critical = 1; // 2 if critical hit

        float STAB = 1.0f; // Same Type Attack Bonus (1.5 if the move is the same type as the user)
        if (caster.Type == battleType.Name) STAB = 1.5f;
        float TypeEffectiveness = battleType.GetBattleTypeInteraction(target.Type);
        float randomMultiplyer = GameManager.rand.Next(217,256)/255;
        float damage = ((((((2 * caster.Level * Critical)/5) + 2) * Power * caster._currentStats[Seraph.Stats.attack] / target._currentStats[Seraph.Stats.defense])/50 + 2) * STAB * TypeEffectiveness * randomMultiplyer);
    }
}
