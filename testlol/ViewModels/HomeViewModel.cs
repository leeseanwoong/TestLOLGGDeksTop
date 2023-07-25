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
using testlol.Managers;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {

        public HomeViewModel()
        {
            League_V4 league_V4 = new League_V4(); // 매니저 만들어서 싱글톤으로 뺴서 사용
            var position = league_V4.GetPosition(UserDataManager.Instance.Summoner);
            if(position.Tier == null)
            {
                TierIcon = "https://z.fow.kr/img/emblem/unranked.png";
                Tier = "UnRanked"; // 이건 스트링을 따로 모아놓는 곳에 저장
                TierIcon = null;
            }
            else
            {
                Tier = position.Tier;
                Rank = position.Rank;
                LeaguePoints = position.leaguePoints + " LP";
                TierIcon = "https://z.fow.kr/img/emblem/" + position.Tier.ToLower() + ".png"; // 상대 경로로 바꾸기, Img경로 따로 뺴서 만들기
            }
            Name = UserDataManager.Instance.UserName;
            SummonerLevel = UserDataManager.Instance.Summoner.SummonerLevel;
            ProfileIconId = "http://opgg-static.akamaized.net/images/profile_icons/profileIcon" + UserDataManager.Instance.Summoner.ProfileIconId + ".jpg";
            
            Wins = position.Wins;
            Losses = position.Losses;
            
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
