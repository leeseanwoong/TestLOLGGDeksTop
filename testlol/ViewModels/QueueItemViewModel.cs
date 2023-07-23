using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.API;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Spectator_V4;

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

        public static QueueItemViewModel From(string compare,PositionDTO position, League_V4 league, CurrentGameParticipantDTO item)
        {
            if(compare == "red")
            {
                return new QueueItemViewModel()
                {
                    tier = position.Tier + " " + position.Rank,
                    SummonerName = item.summonerName,
                    Summoner1Casts = league.GetSpellName((int)item.spell1Id),
                    Summoner2Casts = league.GetSpellName((int)item.spell2Id),
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
                    Summoner1Casts = league.GetSpellName((int)item.spell1Id),
                    Summoner2Casts = league.GetSpellName((int)item.spell2Id),
                    Win = position.Wins + "승 " + position.Losses + "패 " + "(" + string.Format("{0:P0}", (double)position.Wins / (position.Wins + position.Losses)) + ")",
                    PrimaryPerks = item.perks.perksImgs[0],
                    SubPerks = item.perks.perkSubStyleIcon,
                    ChampionName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + item.championName + ".png",
                };
            }
            
        }
    }
}
