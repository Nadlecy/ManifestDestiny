using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class AbilityAttributeAttack : AbilityAttribute
{
    public int Accuracy { get; set; }
    public int CritChance { get; set; }
    public int Power { get; set; }
    public BattleType BattleType { get; set; }

    public AbilityAttributeAttack(int accuracy, int critChance, int power, BattleType battleType) : base()
    {
        Accuracy = accuracy;
        CritChance = critChance;
        Power = power;
        BattleType = battleType;
    }

    public override void Activate(Seraph user, Seraph target)
    {
        if (Power != 0)
        {
            if(GameManager.rand.Next(100) > Accuracy)
            {
                // miss
                GameManager.DialogBubbles.Add(user.Name + " missed");
            }
            else
            {
                int dmg = GameManager.cMaths.DamageCalculator(user, target, Power, CritChance, BattleType);
                target.TakeDamage(dmg);
            }
        }
    }
}

