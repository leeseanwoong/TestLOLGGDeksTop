using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.League_V4
{
    public class PositionDTO
    {
        // 모델을 하나 더 만들어서 그건 이제 get만 있게 해서 생성자에서 값을 받아오는 것으로 수정해야됨 VM <-> M <-> DTO <-> API
        public string Tier { get; set; }
        public string Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string QueueType { get; set; }
        public int leaguePoints { get; set; }
    }
}
