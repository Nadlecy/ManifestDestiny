using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class AbilityAttributeAttackMultiple : AbilityAttribute
{
    public int CritChance { get; set; }
    public int Power { get; set; }
    public BattleType BattleType { get; set; }

    public List<int> StrikeNumber { get; set; }

    public int Accuracy { get; set; }

    public AbilityAttributeAttackMultiple(int accuracy, int critChance, int power, BattleType battleType, List<int> strikeNumber) : base()
    {
        CritChance = critChance;
        Power = power;
        BattleType = battleType;
        StrikeNumber = strikeNumber;
        Accuracy = accuracy;
    }

    public override void Activate(Seraph user, Seraph target)
    {
        if (Power != 0)
        {
            if (GameManager.rand.Next(100) > Accuracy)
            {
                // miss
                GameManager.DialogBubbles.Add(user.Name + " missed");
            } else
            {
                int strikes = GameManager.rand.Next(StrikeNumber[0], StrikeNumber[1]);
                for (int i = 0; i < strikes; i++)
                {
                    int dmg = GameManager.cMaths.DamageCalculator(user, target, Power, CritChance, BattleType);
                    target.TakeDamage(dmg);
                }
                GameManager.DialogBubbles.Add(user.Name + " hit " + strikes + " times");
            }
        }
    }
}

