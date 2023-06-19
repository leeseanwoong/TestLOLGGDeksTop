using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Match_V5;

namespace testlol.ViewModels
{
    internal class RecordListItemViewModel : ViewModelBase
    {
        public string QueueType { get; set; }
        public int Assists { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
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
        public string ChampionPhoto { get; set; }
        public string GameDuration { get; set; }
        public List<ParticipantDTO> BlueTeam { get; set; }
        public List<ParticipantDTO> RedTeam { get; set; }
        public int TotalCS { get; set; }
        public string AvgCS { get; set; }
        public int ChampionLevel { get; set; }
        public string KillRate { get; set; }
        public string TotalGold { get; set; }
        public int TotalDetecedWard { get; set; }
        public string PrimaryPerks { get; set; }
        public string SubPerks { get; set; }
        public long gametime { get; set; }
        public List<TeamDTO> teams { get; set; }
        public PerksDTO Perks { get; set; }
    }
}
