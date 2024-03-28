using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal struct CustomMaths
    {
        public int DamageCalculator(Seraph user, Seraph target, int power, int critChance, BattleType battleType)
        {
            int Critical = 1; // 2 if critical hit
            bool critCheck = (GameManager.rand.Next(0, 100) <= critChance);
            if (critCheck ) Critical = 2;

            float STAB = 1.0f; // Same Type Attack Bonus (1.5 if the move is the same type as the user)
            if (user.Type == battleType) STAB = 1.5f;
            float TypeEffectiveness = battleType.GetBattleTypeInteraction(target.Type);
            float randomMultiplyer = GameManager.rand.Next(217, 256) / 255.0f;
            float damage = ((((((2 * user.Level * Critical) / 5) + 2) * power * user.CurrentStats[Seraph.Stats.attack] / target.CurrentStats[Seraph.Stats.defense]) / 50 + 2) * STAB * TypeEffectiveness * randomMultiplyer);

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

        public float CatchRateCalculator(Seraph enemy, Item ball)
        {
            float status = 1.0f;
            // Status is a multiplier applied if the Pokémon is under the effects of a status condition. Without a status condition, Status is 1×. If the Pokémon is:
            //    asleep or frozen, Status is 2.5×.
            //    paralyzed, burned, or poisoned, Status is 1.5×.
            int n = GameManager.rand.Next(ball.CatchRateMultiplier);
            float f = (enemy.CatchRate * n * status * (1 - (2 / 3) * (enemy.CurrentStats[Seraph.Stats.hp] / enemy.BaseStats[Seraph.Stats.hp])))/255;
            return f;
        }
    }
}
