using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Game
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public StoryProcedureState State { get; set; }

    }
}
