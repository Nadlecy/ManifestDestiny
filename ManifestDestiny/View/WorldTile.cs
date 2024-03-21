using ManifestDestiny.View;
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

        //Dictionary<int, string> _encounters;
        //int _encounterChance;

        public bool IsWarp { get => _isWarp; }
        public ConsoleColor ColorBackground { get => _colorBackground; }
        public ConsoleColor ColorText { get => _colorText; }
        public string Apparence { get => _apparence; }
        public bool Walkable { get => _walkable; }

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
        }
    }
}
