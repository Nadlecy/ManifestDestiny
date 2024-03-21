﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class BattleAttributeAttack : BattleAttribute
    {
        public int CritChance { get; set; }
        public int Power { get; set; }
        public BattleType BattleType { get; set; }

        public BattleAttributeAttack(int critChance, int power, BattleType battleType) :base()
        {
            CritChance = critChance;
            Power = power;
            BattleType = battleType;
        }

        public override void Activate(Seraph user, Seraph target)
        {
            if (Power != 0)
            {
                int dmg = GameManager.cMaths.DamageCalculator(user, target, Power, CritChance, BattleType);
                target.TakeDamage(dmg);
            }
        }
    }

}
