using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class ParticipantDTO
    {
        public int assists { get; set; } //어시
        public int kills { get; set; } //킬
        public int deaths { get; set; } //데스
        public int item0 { get; set; } //아이템
        public int item1 { get; set; }
        public int item2 { get; set; }
        public int item3 { get; set; }
        public int item4 { get; set; }
        public int item5 { get; set; }
        public int item6 { get; set; }
        public string summonerName { get; set; } //소환사 이름
        public bool win { get; set; } //승리 유무
        public int Summoner1Id { get; set; } //스펠1
        public int Summoner2Id { get; set; } //스펠2
        public int playerAugment1 { get; set; } //아레나 스펠1
        public int playerAugment2 { get; set; } //아레나 스펠2
        public int playerAugment3 { get; set; } //아레나 스펠3
        public int playerAugment4 { get; set; } //아레나 스펠4
        public int playerSubteamId { get; set; } //아레나 팀 id
        public int placement { get; set; } // 아레나 등 수
        public string summonerId { get; set; } //소환사 ID값
        public string championName { get; set; } //챔피언 이름
        public PerksDTO perks { get; set; } //룬
        public int teamId { get; set; } //팀ID값
        public string championPhoto { get; set; } //챔피언 사진 주소
        public string KDA { get; set; } //KDA
        public int champLevel { get; set; } //챔피언 레벨
        public int totalMinionsKilled { get; set; } //총 미니언 죽인 개수
        public int neutralMinionsKilled { get; set; } //총 정글몹 죽인개수
        public int totalDamageDealtToChampions { get; set; } //딜량
        public int totalDamageTaken { get; set; } //받은 피해량
        public int detectorWardsPlaced { get; set; } // 제어와드 개수
        public int goldEarned { get; set; } // 총 골드 획득량
        public string lane { get; set; } //라인 top,mid,bottom,jungle
        public string role { get; set; } //역할 solo(미드,탑),support(서폿),carry(원딜),none(정글)
        public string tier { get; set; } //티어
        public int totalCs { get; set; } //총 cs개수
        public double killRate { get; set; } //킬관여율
        public int wardsKilled { get; set; }
        public int wardsPlaced { get; set; }
        public int visionScore { get; set; }

    }
}
