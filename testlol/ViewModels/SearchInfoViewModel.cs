using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Sumonner_V4;

namespace testlol.ViewModels
{
    internal class SearchInfoViewModel : ViewModelBase
    {
        public SearchInfoViewModel()
        {

        }
        public SearchInfoViewModel(SummonerDTO summoner, PositionDTO position)
        {

            SearchName = summoner.SummonerName;
            SummonerLevel = summoner.SummonerLevel;
            ProfileIconId = "http://opgg-static.akamaized.net/images/profile_icons/profileIcon" + summoner.ProfileIconId + ".jpg";
            Tier = position.Tier + " " + position.Rank;
            Wins = position.Wins;
            Losses = position.Losses;
            TierIcon = "C:\\Users\\user\\source\\repos\\testlol\\testlol\\TierIcon\\Tier_" + position.Tier + ".png";
        }
        private string searchName;
        public string SearchName
        {
            get => searchName;
            set => SetProperty(ref searchName, value);
        }
        private string profileIconId;
        public string ProfileIconId
        {

            get => profileIconId;
            set => SetProperty(ref profileIconId, value);
        }

        private long summonerLevel;
        public long SummonerLevel
        {

            get => summonerLevel;
            set => SetProperty(ref summonerLevel, value);
        }
        private string tier;
        public string Tier
        {
            get => tier;
            set => SetProperty(ref tier, value);
        }
        private string tierIcon;
        public string TierIcon
        {
            get => tierIcon;
            set => SetProperty(ref tierIcon, value);
        }
        private int wins;
        public int Wins
        {
            get => wins;
            set => SetProperty(ref wins, value);
        }
        private int losses;
        public int Losses
        {
            get => losses;
            set => SetProperty(ref losses, value);
        }
    }
}
