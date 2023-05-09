using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
