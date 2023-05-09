using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Spectator_V4
{
    public class BanChampionDTO
    {
        public int pickTurn { get; set; }
        public long championId { get; set; }
        public long teamId { get; set; }
    }
}
