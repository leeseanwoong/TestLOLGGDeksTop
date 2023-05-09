using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Sumonner_V4
{
    public class SummonerDTO
    {
        public int ProfileIconId { get; set; }
        public string SummonerName { get; set; }
        public long SummonerLevel { get; set; }
        public string Id { get; set; }
        public string puuid { get; set; }
    }
}
