﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BattleType
{
    string _name;
    Dictionary<string, float> _typeRelations = new Dictionary<string, float>();
    string _statusEffectImmunity;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string StatusEffectImmunity
    {
        get => _statusEffectImmunity;
        set => _statusEffectImmunity = value;
    }

    public BattleType(string typeName)
    {
        _name = typeName;

        _typeRelations.Add("Scramble",1.0f);
        _typeRelations.Add("Occult", 1.0f);
        _typeRelations.Add("Mechanic", 1.0f);
        _typeRelations.Add("Wild", 1.0f);
        _typeRelations.Add("Fluid", 1.0f);
        _typeRelations.Add("Vermin", 1.0f);
        _typeRelations.Add("Absolute", 1.0f);

        switch (_name)
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
                break;

            case "Mechanic":
                _typeRelations["Scramble"] = 0.5f;
                _typeRelations["Occult"] = 2.0f;
                _typeRelations["Mechanic"] = 0.5f;
                _typeRelations["Wild"] = 0.5f;
                _typeRelations["Fluid"] = 2.0f;
                _typeRelations["Vermin"] = 0.0f;
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
                break;

            case "Vermin":
                _typeRelations["Scramble"] = 2.0f;
                _typeRelations["Mechanic"] = 2.0f;
                _typeRelations["Wild"] = 0.5f;
                _typeRelations["Fluid"] = 2.0f;
                _typeRelations["Vermin"] = 0.5f;
                break;

            default:
                _name = "Absolute";
                break;

        }
    }
}
