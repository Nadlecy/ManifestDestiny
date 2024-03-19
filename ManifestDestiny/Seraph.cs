using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Seraph
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int Hp { get; set; }
    public int Power { get; set; }
    public int Resistance { get; set; }
    public int Mana { get; set; }
    public int Magic { get; set; }
    public int Speed { get; set; }
    public string Description { get; set; }

    public Seraph(string name, string type, int hp, int power, int resistance, int mana, int magic, int speed, string description)
    {
        Name = name;
        Type = type;
        Hp = hp;
        Power = power;
        Resistance = resistance;
        Mana = mana;
        Magic = magic;  
        Speed = speed;
        Description = description;
    }
}