using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class ItemStorage
    {
        //public Dictionary<Item, int> Items = new();
        public List<Item> Items = new();
        public ItemStorage() { }

        public void AddItem(Item item, int count = 1)
        {
            while(count > 0)
            {
                bool alreadyHave = false;
                for (int i = 0; i <  Items.Count; i++)
                {
                    if (Items[i].Name == item.Name)
                    {
                        alreadyHave = true;
                        if (Items[i].Count < 99)
                        {
                            Items[i].Count += count;
                            if (Items[i].Count > 99) { Items[i].Count = 99; }
                        }
                    }
                }

                if (alreadyHave == false)
                {
                    Items.Add(item);
                }
                count--;
            }
        }

        public void RemoveItem(Item item, int count = 1)
        {
            while (count > 0)
            {
                if (Items.Contains(item))
                {
                    Items[Items.IndexOf(item)].Count -= count;
                    if (Items[Items.IndexOf(item)].Count == 0)
                    {
                        Items.Remove(item);
                    }
                }
                count--;
            }
        }
    }
}
