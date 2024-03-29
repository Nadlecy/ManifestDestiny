using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleType
{
    public string StatusEffectImmunity{ get; set; }
    public string Name { get; private set; }
    public System.ConsoleColor Color { get; set; }

    public Dictionary<string, float> _typeRelations = new () {
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
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Vermin"] = 2.0f;
                _typeRelations["Fluid"] = 0.0f;
                Color = ConsoleColor.DarkYellow;
                break;

            case "Occult":
                _typeRelations["Occult"] = 0.5f;
                _typeRelations["Mechanic"] = 2.0f;
                _typeRelations["Fluid"] = 2.0f;
                StatusEffectImmunity = "Stun";
                Color = ConsoleColor.DarkMagenta;
                break;

            case "Mechanic":
                _typeRelations["Occult"] = 0.5f;
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Wild"] = 2.0f;
                _typeRelations["Fluid"] = 0.5f;
                _typeRelations["Vermin"] = 2.0f;
                StatusEffectImmunity = "Bleed";
                Color = ConsoleColor.DarkGray;
                break;

            case "Wild":
                _typeRelations["Scramble"] = 2.0f;
                _typeRelations["Occult"] = 2.0f;
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Fluid"] = 0.5f;
                _typeRelations["Vermin"] = 0.5f;
                Color = ConsoleColor.DarkRed;
                break;

            case "Fluid":
                _typeRelations["Scramble"] = 0.5f;
                _typeRelations["Mechanic"] = 2.0f;
                _typeRelations["Wild"] = 0.5f;
                _typeRelations["Vermin"] = 2.0f;
                StatusEffectImmunity = "Burn";
                Color = ConsoleColor.DarkCyan;
                break;

            case "Vermin":
                _typeRelations["Occult"] = 2.0f;
                _typeRelations["Mechanic"] = 0.0f;
                _typeRelations["Wild"] = 2.0f;
                _typeRelations["Vermin"] = 0.5f;
                StatusEffectImmunity = "Poison";
                Color = ConsoleColor.DarkGreen;
                break;

            default:
                Name = "Absolute";
                Color = ConsoleColor.Black;
                break;
        }
    }

    public float GetBattleTypeInteraction(BattleType type)
    {
        return _typeRelations[type.Name];
    }
}
