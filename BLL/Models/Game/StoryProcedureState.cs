using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Game
{
    public class StoryProcedureState
    {
        public int Id { get; set; }
        public Character Character { get; set; }
        public Station Station { get; set; }

    }
}
