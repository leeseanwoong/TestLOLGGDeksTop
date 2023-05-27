using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Spectator_V4
{
    public class CurrentGameParticipantDTO
    {
        public long championid { get; set; }
        public PerkDTO perks { get; set; }
        public long teamid { get; set; }
        public string summonerName { get; set; }
        public long spell1Id { get; set; }
        public long spell2Id { get; set; }
        public string summonerId { get; set; }
        public string championName { get; set; }
    }
}
