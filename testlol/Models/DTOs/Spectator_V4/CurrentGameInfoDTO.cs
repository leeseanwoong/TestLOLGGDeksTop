using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Spectator_V4
{
    public class CurrentGameInfoDTO
    {
        public string gameMode { get; set; }
        public List<BanChampionDTO> bannedChampions { get; set; }
        public List<CurrentGameParticipantDTO> participants { get; set; }
        public long gameQueueConfigId { get; set; }
    }
}
