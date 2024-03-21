using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny.Helper.Math
{
    internal struct CustomMaths
    {
        public int DamageCalculator(Seraph user, Seraph target, int power, int critChance, BattleType battleType)
        {
            int Critical = 1; // 2 if critical hit
            bool critCheck = (GameManager.rand.Next(0, 100) <= critChance);
            if (critCheck ) Critical = 2;

            float STAB = 1.0f; // Same Type Attack Bonus (1.5 if the move is the same type as the user)
            if (user.Type == battleType.Name) STAB = 1.5f;
            float TypeEffectiveness = battleType.GetBattleTypeInteraction(target.Type);
            float randomMultiplyer = GameManager.rand.Next(217, 256) / 255;
            float damage = ((((((2 * user.Level * Critical) / 5) + 2) * power * user._currentStats[Seraph.Stats.attack] / target._currentStats[Seraph.Stats.defense]) / 50 + 2) * STAB * TypeEffectiveness * randomMultiplyer);

            return (int)damage;
        }
        public float StatAlterationMultiplier(Seraph target, Seraph.Stats stat)
        {
            if (target._statsAlterations[stat] < 0)
            {
                return (2.0f / (-1 * target._statsAlterations[stat]));
            }else if (target._statsAlterations[stat] > 0)
            {
                return (target._statsAlterations[stat] / 2.0f);
            }
            else
            {
                return 1.0f;
            }
        }
    }
}
