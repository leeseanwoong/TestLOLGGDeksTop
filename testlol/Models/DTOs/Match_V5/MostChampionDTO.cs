using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class MostChampionDTO
    {
        public int ChampCount { get; set; }
        public string ChampionName { get; set; }
        public string ChampionPhoto { get; set; }
        public int WinCount { get; set; }
        public int LossesCount { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public double KDA { get; set; }
    }
}
