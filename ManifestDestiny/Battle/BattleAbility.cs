using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

class BattleAbility
{
    public string Name { get; set; }
    public BattleType BattleType{ get; set; }
    public int Accuracy { get; set; }
    public int Cost { get; set; }
    public string Description { get; set; }
    public List<AbilityAttribute> attributes {  get; set; }

    public BattleAbility(string name, BattleType type, int accuracy, int cost, string description)
    {
        attributes = new();
        Name = name;
        BattleType = type;
        Accuracy = accuracy;
        Cost = cost;
        Description = description;
    }

    public void AddAttribute(AbilityAttribute attribute)
    {
        attributes.Add(attribute);
    }

    public virtual void Use(Seraph caster, Seraph target) {



        caster._currentStats[Seraph.Stats.mana] -= Cost;
    }
}
