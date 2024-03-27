using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Heal {  get; set; }
        public string Character { get; set; }
        public int Count { get; set; }
        public Item(string name, string description, int count = 1)
        {
            Name = name;
            Description = description;
            Count = count;
        }

        public Item Clone()
        {
            Item it = new Item(Name, Description, Count);
            it.Heal = Heal;
            it.Character = Character;
            return it;
        }

        public virtual void Use()
        {

        }
    }
}
