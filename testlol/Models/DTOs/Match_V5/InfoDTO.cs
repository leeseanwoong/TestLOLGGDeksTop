using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class InfoDTO
    {
        public string gameMode { get; set; }
        public long gameDuration { get; set; }
        public List<ParticipantDTO> participants { get; set; }
        public int queueId { get; set; }
        public List<TeamDTO> teams { get; set; }
    }
}
