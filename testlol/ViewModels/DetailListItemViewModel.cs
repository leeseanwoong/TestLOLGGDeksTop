using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.API;
using testlol.Models.DTOs;
using testlol.Models.DTOs.Match_V5;
using testlol.Utills;

namespace testlol.ViewModels
{
    public class DetailListItemViewModel
    {
        public string tier { get; set; }
        public int ChampionLevel { get; set; }
        public string Item0 { get; set; }
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public string Item4 { get; set; }
        public string Item5 { get; set; }
        public string Item6 { get; set; }
        public string Win { get; set; }
        public string KDA { get; set; }
        public int Ranking { get; set; } // 아레나 등 수
        public string Summoner1Casts { get; set; }
        public string Summoner2Casts { get; set; }
        public string ChampionName { get; set; }
        public string PrimaryPerks { get; set; }
        public string SubPerks { get; set; }
        public string SummonerName { get; set; }
        public int TotalCS { get; set; }
        public string AvgCS { get; set; }
        public string KillRate { get; set; }
        public string TotalGold { get; set; }
        public string TotalDamgeTaken { get; set; }
        public string TotalDamge { get; set; }
        public int MaxDamgeTaken { get; set; }
        public int MaxDamge { get; set; }
        public string visionSocreAndDetectedWards { get; set; }
        public string wardPslacedandkilled { get; set; }
        public string kdaScore { get; set; }

        public static DetailListItemViewModel From(long gameduration, int maxDamge, int maxDamgeTaken, ParticipantDTO participant)
        {
            return new DetailListItemViewModel()
            {
                Item0 =LoLUtility.ReturnItemPhoto(participant.item0),
                Item1 =LoLUtility.ReturnItemPhoto(participant.item1),
                Item2 =LoLUtility.ReturnItemPhoto(participant.item2),
                Item3 =LoLUtility.ReturnItemPhoto(participant.item3),
                Item4 =LoLUtility.ReturnItemPhoto(participant.item4),
                Item5 =LoLUtility.ReturnItemPhoto(participant.item5),
                Item6 = LoLUtility.ReturnItemPhoto(participant.item6),
                SummonerName = participant.summonerName,
                ChampionName = participant.championPhoto,
                TotalGold = string.Format("{0:#,###0G}", participant.goldEarned),
                KDA = participant.KDA,
                kdaScore = participant.kills + " / " + participant.deaths + " / " + participant.assists + "(" + String.Format("{0:P0}", participant.killRate) + ")",
                ChampionLevel = participant.champLevel,
                TotalDamgeTaken = string.Format("{0:#,###0}", participant.totalDamageTaken),
                TotalDamge = string.Format("{0:#,###0}", participant.totalDamageDealtToChampions),
                MaxDamge = maxDamge,
                MaxDamgeTaken = maxDamgeTaken,
                Summoner1Casts = "https://z.fow.kr/img/arena/augment/" + participant.playerAugment1 + ".png",
                Summoner2Casts = "https://z.fow.kr/img/arena/augment/" + participant.playerAugment3 + ".png",
                PrimaryPerks = "https://z.fow.kr/img/arena/augment/" + participant.playerAugment2 + ".png",
                SubPerks = "https://z.fow.kr/img/arena/augment/" + participant.playerAugment4 + ".png",
                Ranking = participant.placement,
                Win = LoLUtility.GetWinLose(participant.win)

            };
        }


        public static DetailListItemViewModel From(string compare,ParticipantDTO participant, long gameduration, int maxDamge, int maxDamgeTaken)
        {
            if (compare == "red")
            {
                return new DetailListItemViewModel()
                {
                    tier = participant.tier,
                    ChampionLevel = participant.champLevel,
                    Summoner1Casts = LoLUtility.GetSpellName(participant.Summoner1Id),
                    Summoner2Casts = LoLUtility.GetSpellName(participant.Summoner2Id),
                    PrimaryPerks = participant.perks.styles[0].selections[0].perkImage,
                    SubPerks = participant.perks.styles[1].styleIcon,
                    Item0 = LoLUtility.ReturnItemPhoto(participant.item0),
                    Item1 = LoLUtility.ReturnItemPhoto(participant.item1),
                    Item2 = LoLUtility.ReturnItemPhoto(participant.item2),
                    Item3 = LoLUtility.ReturnItemPhoto(participant.item3),
                    Item4 = LoLUtility.ReturnItemPhoto(participant.item4),
                    Item5 = LoLUtility.ReturnItemPhoto(participant.item5),
                    Item6 = LoLUtility.ReturnItemPhoto(participant.item6),
                    SummonerName = participant.summonerName,
                    ChampionName = participant.championPhoto,
                    KDA = participant.KDA,
                    TotalDamgeTaken = string.Format("{0:#,###0}", participant.totalDamageTaken),
                    TotalDamge = string.Format("{0:#,###0}", participant.totalDamageDealtToChampions),
                    MaxDamge = maxDamge,
                    MaxDamgeTaken = maxDamgeTaken,
                    TotalCS = participant.totalCs,
                    AvgCS = string.Format("{0:0.0}", participant.totalCs / ((double)gameduration / 60)),
                    TotalGold = string.Format("{0:#,###0G}", participant.goldEarned),
                    visionSocreAndDetectedWards = participant.detectorWardsPlaced + " / " + participant.visionScore,
                    wardPslacedandkilled = participant.wardsPlaced + " / " + participant.wardsKilled,
                    kdaScore = participant.kills + " / " + participant.deaths + " / " + participant.assists + "(" + String.Format("{0:P0}", participant.killRate) + ")",
                };
            }
            else
            {
                return new DetailListItemViewModel()
                {
                    tier = participant.tier,
                    ChampionLevel = participant.champLevel,
                    Summoner1Casts = LoLUtility.GetSpellName(participant.Summoner1Id),
                    Summoner2Casts = LoLUtility.GetSpellName(participant.Summoner2Id),
                    PrimaryPerks = participant.perks.styles[0].selections[0].perkImage,
                    SubPerks = participant.perks.styles[1].styleIcon,
                    Item0 = LoLUtility.ReturnItemPhoto(participant.item0),
                    Item1 = LoLUtility.ReturnItemPhoto(participant.item1),
                    Item2 = LoLUtility.ReturnItemPhoto(participant.item2),
                    Item3 = LoLUtility.ReturnItemPhoto(participant.item3),
                    Item4 = LoLUtility.ReturnItemPhoto(participant.item4),
                    Item5 = LoLUtility.ReturnItemPhoto(participant.item5),
                    Item6 = LoLUtility.ReturnItemPhoto(participant.item6),
                    SummonerName = participant.summonerName,
                    ChampionName = participant.championPhoto,
                    KDA = participant.KDA,
                    TotalDamgeTaken = string.Format("{0:#,###0}", participant.totalDamageTaken),
                    TotalDamge = string.Format("{0:#,###0}", participant.totalDamageDealtToChampions),
                    MaxDamge = maxDamge,
                    MaxDamgeTaken = maxDamgeTaken,
                    TotalCS = participant.totalCs,
                    AvgCS = string.Format("{0:0.0}", participant.totalCs / ((double)gameduration / 60)),
                    TotalGold = string.Format("{0:#,###0G}", participant.goldEarned),
                    visionSocreAndDetectedWards = participant.detectorWardsPlaced + " / " + participant.visionScore,
                    wardPslacedandkilled = participant.wardsPlaced + " / " + participant.wardsKilled,
                    kdaScore = participant.kills + " / " + participant.deaths + " / " + participant.assists + "(" + String.Format("{0:P0}", participant.killRate) + ")",
                };
            }
        }
    }
}
