using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace GoAndFind.ViewModel
{
    public class Item
    {
        public Position Position { get; set; }
        public string Type;
        public string Name{ get; set; }
        public int Ammount { get; set; }
        public Item(Position position, string type,string name,int ammount)
        {
            Position = position;
            Type = type;
            Name = name;
            Ammount = ammount;
        }
    }
}
