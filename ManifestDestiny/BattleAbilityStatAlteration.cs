using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class BattleAbilityStatAlteration : BattleAbility
    {
        /*
        the alteration list parameter must be presented like [a,b,c, x,y,z]
        where the first three numbers are the atk/def/spd of the user
        and the last three are the atk/def/spd of the target
        */
        private List<int> alterationList = new List<int>();

        BattleAbilityStatAlteration(string name, BattleType type, int accuracy, int critChance, int power, int cost, string description, List<int> alterations) : base(name, type, accuracy, critChance, power, cost, description)
        {
            foreach(int num in alterations)
            {
                alterationList.Add(num);
            }
        }

        public override void Use(Seraph caster, Seraph target)
        {
            caster.StatChange(Seraph.Stats.attack, alterationList[0]);
            caster.StatChange(Seraph.Stats.defense, alterationList[1]);
            caster.StatChange(Seraph.Stats.speed, alterationList[2]);
            target.StatChange(Seraph.Stats.attack, alterationList[3]);
            target.StatChange(Seraph.Stats.defense, alterationList[4]);
            target.StatChange(Seraph.Stats.speed, alterationList[5]);

            base.Use(caster, target);
        }
    }
}
