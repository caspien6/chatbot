using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Game
{
    public class User
    {
        public int Id { get; set; }

        public UInt64 Facebook_id { get; set; }

        public string Name { get; set; }

        public ICollection<Story> Stories { get; set; }

    }
}
