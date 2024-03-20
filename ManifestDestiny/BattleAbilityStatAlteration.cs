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
        BattleAbilityStatAlteration(string name, BattleType type, int accuracy, int critChance, int power, int cost, string description) : base(name, type, accuracy, critChance, power, cost, description)
        {
        }

        public override void Use(Seraph caster, Seraph target)
        {
            


            base.Use(caster, target);
        }
    }
}
