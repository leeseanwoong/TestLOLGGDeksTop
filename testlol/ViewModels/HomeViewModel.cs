using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testlol.API;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {

        public HomeViewModel()
        {
            League_V4 league_V4 = new League_V4();
            var position = league_V4.GetPosition(Constants.Summoner);
            Name = Constants.UserName;
            SummonerLevel = Constants.Summoner.SummonerLevel;
            ProfileIconId = "http://opgg-static.akamaized.net/images/profile_icons/profileIcon" + Constants.Summoner.ProfileIconId + ".jpg";
            Tier = position.Tier;
            Rank = position.Rank;
            LeaguePoints = position.leaguePoints + " LP";
            Wins = position.Wins;
            Losses = position.Losses;
            TierIcon = "C:\\Users\\user\\source\\repos\\testlol\\testlol\\TierIcon\\Tier_" + position.Tier + ".png";
        }

        #region property
        private string name;
        public string Name
        {

            get => name;
            set => SetProperty(ref name, value);
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
        private string rank;
        public string Rank
        {
            get => rank;
            set => SetProperty(ref rank, value);
        }
        private string leaguePoints;
        public string LeaguePoints
        {
            get => leaguePoints;
            set => SetProperty(ref leaguePoints, value);
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
        #endregion


    }
}
