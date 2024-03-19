using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleType
{
    public string StatusEffectImmunity{ get; set; }
    public string Name { get; private set; }

    Dictionary<string, float> _typeRelations = new Dictionary<string, float>()
    {
        {"Scramble",1.0f},
        {"Occult", 1.0f},
        {"Mechanic", 1.0f},
        {"Wild", 1.0f},
        {"Fluid", 1.0f},
        {"Vermin", 1.0f},
        {"Absolute", 1.0f},
    };

    public BattleType(string typeName)
    {
        StatusEffectImmunity = "";
        Name = typeName;

        switch (Name)
        {
            case "Scramble":
                _typeRelations["Wild"] = 2.0f;
                _typeRelations["Fluid"] = 0.5f;
                break;

            case "Occult":
                _typeRelations["Occult"] = 0.5f;
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Wild"] = 2.0f;
                _typeRelations["Vermin"] = 2.0f;
                StatusEffectImmunity = "Stun";
                break;

            case "Mechanic":
                _typeRelations["Scramble"] = 0.5f;
                _typeRelations["Occult"] = 2.0f;
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Wild"] = 0.5f;
                _typeRelations["Fluid"] = 2.0f;
                _typeRelations["Vermin"] = 0.0f;
                StatusEffectImmunity = "Bleed";
                break;

            case "Wild":
                _typeRelations["Mechanic"] = 2.0f;
                _typeRelations["Fluid"] = 0.5f;
                _typeRelations["Vermin"] = 2.0f;
                break;

            case "Fluid":
                _typeRelations["Scramble"] = 0.0f;
                _typeRelations["Occult"] = 2.0f;
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Wild"] = 0.5f;
                StatusEffectImmunity = "Burn";
                break;

            case "Vermin":
                _typeRelations["Scramble"] = 2.0f;
                _typeRelations["Mechanic"] = 2.0f;
                _typeRelations["Wild"] = 0.5f;
                _typeRelations["Fluid"] = 2.0f;
                _typeRelations["Vermin"] = 0.5f;
                StatusEffectImmunity = "Poison";
                break;

            default:
                Name = "Absolute";
                break;
        }
    }

    public float GetBattleTypeInteraction(string type)
    {
        return _typeRelations[type];
    }
}
