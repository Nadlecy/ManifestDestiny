﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal struct CustomMaths
    {
        public int DamageCalculator(Seraph user, Seraph target, BattleAbility battleAbility)
        {
            int Critical = 1; // 2 if critical hit
            bool critCheck = (GameManager.rand.Next(0, 100) <= battleAbility.CritChance);
            if (critCheck ) Critical = 2;

            float STAB = 1.0f; // Same Type Attack Bonus (1.5 if the move is the same type as the user)
            if (user.Type == battleAbility.BattleType.Name) STAB = 1.5f;
            float TypeEffectiveness = battleAbility.BattleType.GetBattleTypeInteraction(target.Type);
            float randomMultiplyer = GameManager.rand.Next(217, 256) / 255;
            float damage = ((((((2 * user.Level * Critical) / 5) + 2) * battleAbility.Power * user._currentStats[Seraph.Stats.attack] / target._currentStats[Seraph.Stats.defense]) / 50 + 2) * STAB * TypeEffectiveness * randomMultiplyer);

            return (int)damage;
        }
    }
}
