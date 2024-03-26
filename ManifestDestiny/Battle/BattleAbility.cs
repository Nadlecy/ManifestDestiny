using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
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

    //get attribute function
    public virtual void Use(Seraph caster, Seraph target)
    {
        //activate the effect of every attribute
        foreach(var attribute in attributes)
        {
            attribute.Activate(caster, target);
        }
        //remove the corresponding amount of mana
        caster.CurrentStats[Seraph.Stats.mana] -= Cost;
    }
}

class Attribute<T> where T : AbilityAttribute
{

} 