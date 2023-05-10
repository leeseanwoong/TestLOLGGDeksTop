using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class PerkStatsDTO
    {
        public int defense { get; set; } // 맨 아래쪽
        public string defenseImg { get; set; }
        public string defenseDesc { get; set; }
        public int flex { get; set; } // 중앙
        public string flexImg { get; set; }
        public string flexDesc { get; set; }
        public int offense { get; set; } // 맨 위쪽
        public string offenseImg { get; set; }
        public string offenseDesc { get; set; }
    }
}
