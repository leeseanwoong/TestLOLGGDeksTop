using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.API;
using testlol.Models.DTOs.Match_V5;

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

        public static DetailListItemViewModel From(string compare, Match_V5 match_V5, List<ParticipantDTO> Red, List<ParticipantDTO> Blue, int i, long gameduration, int maxDamge, int maxDamgeTaken)
        {
            if (compare == "red")
            {
                return new DetailListItemViewModel()
                {
                    tier = Red[i].tier,
                    ChampionLevel = Red[i].champLevel,
                    Summoner1Casts = match_V5.GetSpellName(Red[i].Summoner1Id),
                    Summoner2Casts = match_V5.GetSpellName(Red[i].Summoner2Id),
                    PrimaryPerks = Red[i].perks.styles[0].selections[0].perkImage,
                    SubPerks = Red[i].perks.styles[1].styleIcon,
                    Item0 = match_V5.ReturnItemPhoto(Red[i].item0),
                    Item1 = match_V5.ReturnItemPhoto(Red[i].item1),
                    Item2 = match_V5.ReturnItemPhoto(Red[i].item2),
                    Item3 = match_V5.ReturnItemPhoto(Red[i].item3),
                    Item4 = match_V5.ReturnItemPhoto(Red[i].item4),
                    Item5 = match_V5.ReturnItemPhoto(Red[i].item5),
                    Item6 = match_V5.ReturnItemPhoto(Red[i].item6),
                    SummonerName = Red[i].summonerName,
                    ChampionName = Red[i].championPhoto,
                    KDA = Red[i].KDA,
                    TotalDamgeTaken = string.Format("{0:#,###0}", Red[i].totalDamageTaken),
                    TotalDamge = string.Format("{0:#,###0}", Red[i].totalDamageDealtToChampions),
                    MaxDamge = maxDamge,
                    MaxDamgeTaken = maxDamgeTaken,
                    TotalCS = Red[i].totalCs,
                    AvgCS = string.Format("{0:0.0}", Red[i].totalCs / ((double)gameduration / 60)),
                    TotalGold = string.Format("{0:#,###0G}", Red[i].goldEarned),
                    visionSocreAndDetectedWards = Red[i].detectorWardsPlaced + " / " + Red[i].visionScore,
                    wardPslacedandkilled = Red[i].wardsPlaced + " / " + Red[i].wardsKilled,
                    kdaScore = Red[i].kills + " / " + Red[i].deaths + " / " + Red[i].assists + "(" + String.Format("{0:P0}", Red[i].killRate) + ")",
                };
            }
            else
            {
                return new DetailListItemViewModel()
                {
                    tier = Blue[i].tier,
                    ChampionLevel = Blue[i].champLevel,
                    Summoner1Casts = match_V5.GetSpellName(Blue[i].Summoner1Id),
                    Summoner2Casts = match_V5.GetSpellName(Blue[i].Summoner2Id),
                    PrimaryPerks = Blue[i].perks.styles[0].selections[0].perkImage,
                    SubPerks = Blue[i].perks.styles[1].styleIcon,
                    Item0 = match_V5.ReturnItemPhoto(Blue[i].item0),
                    Item1 = match_V5.ReturnItemPhoto(Blue[i].item1),
                    Item2 = match_V5.ReturnItemPhoto(Blue[i].item2),
                    Item3 = match_V5.ReturnItemPhoto(Blue[i].item3),
                    Item4 = match_V5.ReturnItemPhoto(Blue[i].item4),
                    Item5 = match_V5.ReturnItemPhoto(Blue[i].item5),
                    Item6 = match_V5.ReturnItemPhoto(Blue[i].item6),
                    SummonerName = Blue[i].summonerName,
                    ChampionName = Blue[i].championPhoto,
                    KDA = Blue[i].KDA,
                    TotalDamgeTaken = string.Format("{0:#,###0}", Blue[i].totalDamageTaken),
                    TotalDamge = string.Format("{0:#,###0}", Blue[i].totalDamageDealtToChampions),
                    MaxDamge = maxDamge,
                    MaxDamgeTaken = maxDamgeTaken,
                    TotalCS = Blue[i].totalCs,
                    AvgCS = string.Format("{0:0.0}", Blue[i].totalCs / ((double)gameduration / 60)),
                    TotalGold = string.Format("{0:#,###0G}", Blue[i].goldEarned),
                    visionSocreAndDetectedWards = Blue[i].detectorWardsPlaced + " / " + Blue[i].visionScore,
                    wardPslacedandkilled = Blue[i].wardsPlaced + " / " + Blue[i].wardsKilled,
                    kdaScore = Blue[i].kills + " / " + Blue[i].deaths + " / " + Blue[i].assists + "(" + String.Format("{0:P0}", Blue[i].killRate) + ")",
                };
            }
        }
    }
}
