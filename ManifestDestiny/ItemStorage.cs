﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class ItemStorage
    {
        public Dictionary<Item, int> Items = new();
        public ItemStorage() { }

        public void AddItem(Item item, int count = 1)
        {
            while(count > 0)
            {
                if (Items.ContainsKey(item))
                {
                    if (Items[item] < 99)
                    {
                        Items[item] += 1;
                    }
                }
                else
                {
                    Items.Add(item, 1);
                }
                count--;
            }
        }

        public void RemoveItem(Item item, int count = 1)
        {
            while (count > 0)
            {
                if (Items.ContainsKey(item))
                {
                    Items[item] -= 1;
                    if (Items[item] == 0)
                    {
                        Items.Remove(item);
                    }
                }
                count--;
            }
        }

        public List<string> getList()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<Item, int> entry in Items)
            {
                list.Add(entry.Key.Name + " x" + entry.Value);
            }
            return list;
        }
    }
}
