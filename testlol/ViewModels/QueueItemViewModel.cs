using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.API;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Spectator_V4;
using testlol.Utills;

namespace testlol.ViewModels
{
    public class QueueItemViewModel
    {
        public string tier { get; set; }
        public string Win { get; set; }
        public string Summoner1Casts { get; set; }
        public string Summoner2Casts { get; set; }
        public string ChampionName { get; set; }
        public string PrimaryPerks { get; set; }
        public string SubPerks { get; set; }
        public string SummonerName { get; set; }
        public string BanChampionName { get; set; }

        public static QueueItemViewModel From(string compare, CurrentGameParticipantDTO item)
        {
            League_V4 league = new League_V4();
            Match_V5 match_V5 = new Match_V5();
            Spectator_V4 spectator_V4 = new Spectator_V4();

            ChampionsDTO champions = spectator_V4.GetChampions();
            List<RuneDTO> rune = match_V5.GetRune();

            var position = league.GetPositions(item.summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
            if (position == null)
            {
                position = new Models.DTOs.League_V4.PositionDTO();
                position.Tier = "UnRanked";
            }

            LoLUtility.GetPerks(item.perks, rune);

            foreach (var data in champions.data) // 챔피언 아이디 값으로 이름 찾기
            {
                if (long.Parse(champions.data[data.Key].key) == item.championid)
                {
                    item.championName = champions.data[data.Key].id;
                }

            }

            if (compare == "red")
            {
                return new QueueItemViewModel()
                {
                    tier = position.Tier + " " + position.Rank,
                    SummonerName = item.summonerName,
                    Summoner1Casts = LoLUtility.GetSpellName((int)item.spell1Id),
                    Summoner2Casts = LoLUtility.GetSpellName((int)item.spell2Id),
                    Win = position.Wins + "승 " + position.Losses + "패 " + "(" + string.Format("{0:P0}", (double)position.Wins / (position.Wins + position.Losses)) + ")",
                    PrimaryPerks = item.perks.perksImgs[0],
                    SubPerks = item.perks.perkSubStyleIcon,
                    ChampionName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + item.championName + ".png",
                };
            }
            else
            {
                return new QueueItemViewModel()
                {
                    tier = position.Tier + " " + position.Rank,
                    SummonerName = item.summonerName,
                    Summoner1Casts = LoLUtility.GetSpellName((int)item.spell1Id),
                    Summoner2Casts = LoLUtility.GetSpellName((int)item.spell2Id),
                    Win = position.Wins + "승 " + position.Losses + "패 " + "(" + string.Format("{0:P0}", (double)position.Wins / (position.Wins + position.Losses)) + ")",
                    PrimaryPerks = item.perks.perksImgs[0],
                    SubPerks = item.perks.perkSubStyleIcon,
                    ChampionName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + item.championName + ".png",
                };
            }
            
        }
    }
}
