using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.Game
{
    public class Station
    {
        public int Id { get; set; }

        public string Story { get; set; }

        public IDictionary<string,Station> NextStations { get; set; }

    }
}
