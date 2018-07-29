using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Game
{
    public class StoryPool
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Station StarterStation { get; set; }

    }
}
