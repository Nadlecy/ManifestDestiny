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

        public enum MenuDisplayType
        {
            rightSide, // Pokemon style menu
            leftSide, //Pokemon style menu but on the left side
            bottom,
            battle, // choice of action at start of round
            abilities // choice of attacks during battle
        }

        public string Name { get; set; }
        public List<string> _lines;
        public ItemStorage? ItemStorage { get; set; }
        public List<BattleAbility>? Abilities {  get; set; }
        public List<Seraph>? Seraphim {  get; set; }
        public int SelectedLine { get; set; }
        public LinesType LineType { get; private set; }
        public MenuDisplayType DisplayType { get; private set; }

        public Seraph seraph1 { get; set; }
        public Seraph seraph2 { get; set; }

        // Creates a menu with a name and a dictionary of lines. A default menu is called "Menu" and has 1 line called "close"
        public Menu(string name = "MENU", List<string> lines = null, MenuDisplayType displayType = MenuDisplayType.leftSide)
        {
            LineType = LinesType.text;
            DisplayType = displayType;
            SelectedLine = 0;
            Name = name;
            if (lines == null)
            {
                _lines = new List<string>() { "CLOSE" }; // Default menu only has close option
            }
            else { _lines = lines; }
            DisplayType = displayType;
        }

        public Menu(string name, ItemStorage itemStorage, MenuDisplayType displayType = MenuDisplayType.leftSide)
        {
            LineType = LinesType.items;
            DisplayType = displayType;
            Name = name;
            ItemStorage = itemStorage;

            // Select first item
            SelectedLine = 0;
        }

        public Menu(string name, List<BattleAbility> abilities, MenuDisplayType displayType = MenuDisplayType.abilities)
        {
            LineType = LinesType.ability;
            DisplayType = displayType;
            Name = name;
            Abilities = abilities;

            // Select first item
            SelectedLine = 0;
        }

        public Menu(string name, List<Seraph> seraphim, MenuDisplayType displayType = MenuDisplayType.leftSide)
        {
            LineType = LinesType.seraph;
            DisplayType = displayType; 
            Name = name;
            Seraphim = seraphim;

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
                    if (SelectedLine > Abilities.Count)
                    {
                        SelectedLine = 0;
                    }
                    break;
                case LinesType.seraph:
                    if (SelectedLine > Seraphim.Count)
                    {
                        SelectedLine = 0;
                    }
                    break;
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
                } else if (LineType == LinesType.seraph)
                {
                    SelectedLine = Seraphim.Count;
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
                    if (SelectedLine == Abilities.Count)
                    {
                        return "CLOSE";
                    }
                    return Abilities[SelectedLine].Name;
                case LinesType.seraph:
                    if(SelectedLine == Seraphim.Count)
                    {
                        return "CLOSE";
                    }

                    if(seraph1 == null && Seraphim[SelectedLine] != seraph2)
                    {
                        seraph1 = Seraphim[SelectedLine];
                    } else if(seraph2 == null && Seraphim[SelectedLine] != seraph1)
                    {
                        seraph2 = Seraphim[SelectedLine];
                    } else
                    {
                        //return Seraphim[SelectedLine].Name;
                    }

                    if(seraph1 != null && seraph2 != null)
                    {
                        Swap(Seraphim, Seraphim.IndexOf(seraph1), Seraphim.IndexOf(seraph2));

                        seraph1 = null;
                        seraph2 = null;
                    }

                    return "Switched two seraph";

                default :
                    return "default";
            }
            
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB) // https://stackoverflow.com/questions/2094239/swap-two-items-in-listt
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}
