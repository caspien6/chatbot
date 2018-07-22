using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Models.Game
{
    public class Character
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public ICollection<Item> Items { get; set; }
        
    }
}
