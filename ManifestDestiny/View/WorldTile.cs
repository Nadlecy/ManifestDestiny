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
        //Dictionary<int, string> _encounters;
        //int _encounterChance;

        public ConsoleColor ColorBackground { get => _colorBackground; }
        public ConsoleColor ColorText { get => _colorText; }
        public string Apparence { get => _apparence; }
        public bool Walkable { get => _walkable; }

        public WorldTile(string apparence, ConsoleColor colorBackground, ConsoleColor colorText, bool walkable)
        {
            _apparence = apparence;
            _walkable = walkable;
            _colorBackground = colorBackground;
            _colorText = colorText;
        }
    }
}
