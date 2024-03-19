using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleType
{
    public string _name;
    public Dictionary<string, float> _typeRelations = new Dictionary<string, float>();
    public string _statusEffectImmunity;

    public BattleType(string typeName)
    {
        _name = typeName;

        _typeRelations.Add("Scramble",1.0f);

        switch (_name)
        {
            case "Scramble":

                break;

            case "Occult":

                break;

            case "Mechanic":

                break;

            case "Wild":

                break;

            case "Fluid":

                break;

            case "Vermin":

                break;

            default:
                _name = "Absolute";
                break;

        }
    }
}
