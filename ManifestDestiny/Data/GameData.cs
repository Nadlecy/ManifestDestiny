using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ManifestDestiny.Helper.Json;
using ManifestDestiny.Container;

namespace ManifestDestiny
{
    internal struct GameData
    {
        public Dictionary<string, BattleType> battleTypes;
        public Dictionary<string, BattleAbility> abilities;
        public Dictionary<string, Seraph> seraphim;

        public GameData()
        {
            battleTypes = new Dictionary<string, BattleType>();
            abilities = new Dictionary<string, BattleAbility>();
            seraphim = new Dictionary<string, Seraph>();

            CreateTypes();
            CreateAbilities();
            CreateSeraphim();
        }

        private void CreateTypes()
        {
            battleTypes.Add("Scramble", new BattleType("Scramble"));
            battleTypes.Add("Occult", new BattleType("Occult"));
            battleTypes.Add("Mechanic", new BattleType("Mechanic"));
            battleTypes.Add("Wild", new BattleType("Wild"));
            battleTypes.Add("Fluid", new BattleType("Fluid"));
            battleTypes.Add("Vermin", new BattleType("Vermin"));
            battleTypes.Add("Absolute", new BattleType("Absolute"));
        }

        private void CreateAbilities()
        {

            string pathAbility = "Seraph/Ability.json";
            CustomJson<AbilityContainer> jsonReader = new CustomJson<AbilityContainer>(pathAbility);
            AbilityContainer abilityContain = jsonReader.Read();

            foreach (AbilityData ability in abilityContain.Ability)
            {
                abilities.Add(ability.Name, new BattleAbility(ability.Name, battleTypes[ability.Type], ability.Accuracy, ability.ManaCost, ability.Description));
                foreach (string attribute in ability.Attributes)
                {
                    switch (attribute)
                    {
                        case "AttributeAttack":
                            abilities[ability.Name].AddAttribute(new AbilityAttributeAttack((int)ability.CritChance, (int)ability.Power, battleTypes[ability.Type]));
                            break;
                    }
                }

            }
            /*
            abilities.Add("Bash", new BattleAbility("Bash", battleTypes["Scramble"], 95, 4, ""));
            abilities["Bash"].AddAttribute(new AbilityAttributeAttack(10, 40, battleTypes["Scramble"]));
            */
        }

        private void CreateSeraphim()
        {
            string pathSeraph = "Seraph/Seraph.json";
            CustomJson<SeraphContainer> jsonReader = new CustomJson<SeraphContainer>(pathSeraph);
            SeraphContainer seraphimContain = jsonReader.Read();

            foreach(SeraphData seraph in seraphimContain.Seraph)
            {
                seraphim.Add(seraph.Name, new Seraph(
                    seraph.Name,
                    battleTypes[seraph.Type],
                    seraph.CatchRate,
                    new Dictionary<Seraph.Stats, int>
                    {
                        {Seraph.Stats.hp, seraph.HP},
                        {Seraph.Stats.attack, seraph.Attack},
                        {Seraph.Stats.defense, seraph.Defense},
                        {Seraph.Stats.mana, seraph.Mana},
                        {Seraph.Stats.magic, seraph.Magic},
                        {Seraph.Stats.speed, seraph.Speed}
                    }, new Dictionary<Seraph.Stats, int>
                    {
                        {Seraph.Stats.hp, seraph.HP100},
                        {Seraph.Stats.attack, seraph.Attack100},
                        {Seraph.Stats.defense, seraph.Defense100},
                        {Seraph.Stats.mana, seraph.Mana100},
                        {Seraph.Stats.magic, seraph.Magic100},
                        {Seraph.Stats.speed, seraph.Speed100}
                    }, seraph.ExpReward,
                    new Dictionary<int, BattleAbility>(),
                    seraph.Description
                ));

                foreach (KeyValuePair<int, string> pair in seraph.Ability)
                {
                    seraphim[seraph.Name]._abilitiesUnlocks.Add(pair.Key, abilities[pair.Value]);
                }
            }   
        }

        public Seraph Summon(string name, int level)
        {
            Seraph newGuy = seraphim[name].Clone();
            newGuy.Experience = level * 100;

            newGuy.FullHeal();

            return newGuy;
        }
    }
}
