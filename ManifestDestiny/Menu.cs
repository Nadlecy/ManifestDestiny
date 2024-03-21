using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Menu
    {
        public string Name { get; set; }
        public List<string> _lines;
        public int SelectedLine { get; set; }

        // Creates a menu with a name and a dictionary of lines. A default menu is called "Menu" and has 1 line called "close"
        public Menu(string name = "MENU", List<string> lines = null)
        {
            SelectedLine = 0;
            Name = name;
            if (lines == null)
            {
                _lines = new List<string>(){"CLOSE"}; // Default menu only has close option
            } else { _lines = lines; }
        }

        // Select the next line in the menu
        public virtual void NextLine()
        {
            SelectedLine++;
            if(SelectedLine >= _lines.Count) { 
                SelectedLine = 0;
            }
        }

        // Select the previous line in the menu
        public virtual void PreviousLine()
        {
            SelectedLine--;
            if (SelectedLine < 0)
            {
                SelectedLine = _lines.Count - 1;
            }
        }

        // Confirm line selection
        public virtual string Enter()
        {
            return _lines[SelectedLine];
        }
    }
}
