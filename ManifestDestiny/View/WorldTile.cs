using ManifestDestiny;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class WorldTile
    {
        string _apparence;
        bool _walkable;
        ConsoleColor _colorBackground;
        ConsoleColor _colorText;
        bool _isWarp;
        Warp _warp;

        public bool IsWarp { get => _isWarp; }
        public ConsoleColor ColorBackground { get => _colorBackground; }
        public ConsoleColor ColorText { get => _colorText; }
        public string Apparence { get => _apparence; }
        public bool Walkable { get => _walkable; }
        public Dictionary<string, int> Encounters { get; set; }
        public int EncounterChance { get; set; }
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public int AILevel { get; set; }
        public bool AsObject { get; set; }
        public Item Item { get; set; }

        public Warp Warp { get => _warp; }

        public void SetWarp(Warp warp)
        {
            _warp = warp;
            _isWarp = true;
        }

        public WorldTile(string apparence, ConsoleColor colorBackground, ConsoleColor colorText, bool walkable)
        {
            _apparence = apparence;
            _walkable = walkable;
            _colorBackground = colorBackground;
            _colorText = colorText;
            _isWarp = false;
            EncounterChance = 0;
        }

        public Item GiveObject()
        {
            AsObject = false;
            return Item;
        }

        public WorldTile Clone()
        {
            var wt = new WorldTile(_apparence, _colorBackground, _colorText, _walkable);

            return wt;

        }
    }
}
