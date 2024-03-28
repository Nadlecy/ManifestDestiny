﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

class AbilityAttributeStatAlteration : AbilityAttribute
{
    /*
    the alteration list parameter must be presented like [a,b,c,d, w,x,y,z]
    where the first three numbers are the atk/def/spd of the user
    and the last three are the atk/def/spd of the target
    */
    private List<int> alterationList;

    public AbilityAttributeStatAlteration(List<int> alterations) : base()
    {
        alterationList = new List<int>();

        foreach (int num in alterations)
        {
            alterationList.Add(num);
        }
    }

    public override void Activate(Seraph caster, Seraph target)
    {
        caster.StatChange(Seraph.Stats.attack, alterationList[0]);
        caster.StatChange(Seraph.Stats.defense, alterationList[1]);
        caster.StatChange(Seraph.Stats.speed, alterationList[2]);
        caster.StatChange(Seraph.Stats.magic, alterationList[3]);
        target.StatChange(Seraph.Stats.attack, alterationList[4]);
        target.StatChange(Seraph.Stats.defense, alterationList[5]);
        target.StatChange(Seraph.Stats.speed, alterationList[6]);
        target.StatChange(Seraph.Stats.magic, alterationList[7]);
    }
}
