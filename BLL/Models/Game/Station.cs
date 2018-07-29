using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Game
{
    public class Station
    {
        public int Id { get; set; }

        public string ConnectionCommand { get; set; }

        public string Story { get; set; }

        public ICollection<Station> NextStations { get; set; }

    }
}
