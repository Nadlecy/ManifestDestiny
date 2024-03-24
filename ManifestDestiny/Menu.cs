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
        public int SelectedLine { get; set; }
        public Item SelectedItem { get; set; }
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
            foreach (KeyValuePair<Item, int> entry in ItemStorage.Items)
            {
                if (SelectedItem == null) { SelectedItem = entry.Key; }
            }
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
                case LinesType.items:
                    bool selectNext = false;
                    foreach (KeyValuePair<Item, int> entry in ItemStorage.Items)
                    {
                        if (selectNext)
                        {
                            SelectedItem = entry.Key;
                        }
                        if (SelectedItem == entry.Key) {
                            selectNext = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        // Select the previous line in the menu
        public virtual void PreviousLine()
        {
            SelectedLine--;
            switch (LineType)
            {
                case LinesType.text:
                    if (SelectedLine < 0)
                    {
                        SelectedLine = _lines.Count - 1;
                    }
                    break;
                case LinesType.items:
                    bool selectPrevious = false;
                    foreach (KeyValuePair<Item, int> entry in ItemStorage.Items.Reverse())
                    {
                        if (selectPrevious)
                        {
                            SelectedItem = entry.Key;
                        }
                        if (SelectedItem == entry.Key)
                        {
                            selectPrevious = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        // Confirm line selection
        public virtual string Enter()
        {
            return _lines[SelectedLine];
        }
    }
}
