using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            abilities.Add("Bash", new BattleAbility("Bash", battleTypes["Scramble"], 95, 4, ""));
            abilities["Bash"].AddAttribute(new AbilityAttributeAttack(10, 40, battleTypes["Scramble"]));
        }

        private void CreateSeraphim()
        {
            //creating Lambda
            seraphim.Add("Lambda", new Seraph("Lambda",
                battleTypes["Absolute"],
                new Dictionary<Seraph.Stats, int>
                {
                    { Seraph.Stats.hp, 35 },
                    { Seraph.Stats.attack, 28 },
                    { Seraph.Stats.defense, 19 },
                    { Seraph.Stats.mana, 36 },
                    { Seraph.Stats.magic, 7 },
                    { Seraph.Stats.speed, 13 }
                },
                20,
                new Dictionary<int, int>
                {
                    { 1, 0 },
                    { 2, 20 },
                    { 3, 52 },
                    { 4, 112 },
                    { 5, 184 },
                },
                new Dictionary<int, BattleAbility>
                {
                    { 1, abilities["Bash"] },
                },
                "Average monster"
                ));

        }

        public Seraph Summon(string name, int level)
        {
            Seraph newGuy = seraphim[name].Copy();
            newGuy.Experience = newGuy._xpForLevel[level];
            newGuy.GainLevelSkills();           

            return newGuy;
        }
    }
}
