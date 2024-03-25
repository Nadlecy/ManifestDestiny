using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class SeraphContainer
    {
        Dictionary<string, SeraphData> _seraph;
    }

    internal class SeraphData
    {
        string _name;
        string _type;
        int _hp;
        int _atk;
        int _def;
        int _spd;
        int _mana;
        int _magic;

        int _hp100;
        int _atk100;
        int _def100;
        int _spd100;
        int _mana100;
        int _magic100;

        int _expReward;
        Dictionary<int, string> _ability;
        string _description;
    }
}
