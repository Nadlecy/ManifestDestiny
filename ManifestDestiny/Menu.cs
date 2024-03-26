using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Menu
    {
        public enum LinesType
        {
            text,
            items,
            seraph,
            ability
        }

        public string Name { get; set; }
        public List<string> _lines;
        public ItemStorage ItemStorage { get; set; }
        public List<BattleAbility> Abilities {  get; set; }
        public List<SeraphContainer> Seraphim {  get; set; }
        public int SelectedLine { get; set; }
        public LinesType LineType { get; private set; }

        // Creates a menu with a name and a dictionary of lines. A default menu is called "Menu" and has 1 line called "close"
        public Menu(string name = "MENU", List<string> lines = null)
        {
            LineType = LinesType.text;
            SelectedLine = 0;
            Name = name;
            if (lines == null)
            {
                _lines = new List<string>(){"CLOSE"}; // Default menu only has close option
            } else { _lines = lines; }
        }

        public Menu(string name, ItemStorage itemStorage)
        {
            LineType = LinesType.items;
            Name = name;
            ItemStorage = itemStorage;

            // Select first item
            SelectedLine = 0;
        }

        public Menu(string name, List<BattleAbility> abilities)
        {
            LineType = LinesType.ability;
            Name = name;
            Abilities = abilities;

            // Select first item
            SelectedLine = 0;
        }

        public Menu(string name, List<SeraphContainer> seraphim)
        {
            LineType = LinesType.ability;
            Name = name;
            Seraphim = seraphim;

            for(SeraphContainer seraph in seraphim)
            {
                _lines.Add(seraph.Name)
            }

            // Select first item
            SelectedLine = 0;
        }

        // Select the next line in the menu
        public virtual void NextLine()
        {
            SelectedLine++;
            switch (LineType)
            {
                case LinesType.text:
                    if (SelectedLine >= _lines.Count)
                    {
                        SelectedLine = 0;
                    }
                    break;
                case LinesType.ability:
                case LinesType.items:
                    if(SelectedLine > ItemStorage.Items.Count)
                    {
                        SelectedLine = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        // Select the previous line in the menu
        public virtual void PreviousLine()
        {
            //Console.WriteLine("MAOGFGIS");
            SelectedLine--;
            if (SelectedLine < 0)
            {
                if(LineType == LinesType.text)
                {
                    SelectedLine = _lines.Count - 1;
                }
                else if (LineType == LinesType.items)
                {
                    SelectedLine = ItemStorage.Items.Count;
                } else if(LineType == LinesType.ability)
                {
                    SelectedLine = Abilities.Count;
                }
            }
        }

        // Confirm line selection
        public virtual string Enter()
        {
            switch (LineType)
            {
                case LinesType.text:
                    return _lines[SelectedLine];
                case LinesType.items:
                    if (SelectedLine == ItemStorage.Items.Count)
                    {
                        return "CLOSE";
                    }
                    Console.SetCursorPosition(0, ItemStorage.Items.Count + 2);
                    Console.WriteLine(ItemStorage.Items[SelectedLine].Description);
                    return ItemStorage.Items[SelectedLine].Description;
                case LinesType.ability:
                    return Abilities[SelectedLine].Name;
                    break;
                default :
                    return "default";
            }
            
        }
    }
}
