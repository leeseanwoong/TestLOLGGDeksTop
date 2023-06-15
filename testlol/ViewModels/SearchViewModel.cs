using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows.Input;
using testlol.Utills;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.API;
using System.Windows;
using System.Collections.ObjectModel;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.League_V4;

namespace testlol.ViewModels
{
    internal class SearchViewModel : ViewModelBase
    {
        public SearchViewModel()
        {

            
            
        }
        
        #region properties
        private string summonerName;
        public string SummonerName
        {
            get => summonerName;
            set => SetProperty(ref summonerName, value);
        }

        private SummonerDTO summoner;
        public SummonerDTO Summoner
        {
            get => summoner;
            set => SetProperty(ref summoner, value);
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

        /*private ObservableCollection<RecordListItemViewModel> innerItems { get; } = new ObservableCollection<RecordListItemViewModel>();
        private ReadOnlyObservableCollection<RecordListItemViewModel> items;
        public ReadOnlyObservableCollection<RecordListItemViewModel> Items
        {
            get
            {
                if (items == null)
                {
                    items = new ReadOnlyObservableCollection<RecordListItemViewModel>(innerItems);
                    Match_V5 match_V5 = new Match_V5();
                    string matchlist = match_V5.GetMatchList(Constants.Summoner.puuid);
                    List<RuneDTO> rune = match_V5.GetRune();
                    matchlist = matchlist.Replace("\"", "");
                    matchlist = matchlist.Replace("[", "");
                    matchlist = matchlist.Replace("]", "");
                    string[] arr = matchlist.Split(",");

                    for (int i = 0; i < arr.Length; i++)
                    {
                        List<ParticipantDTO> redTeam = new List<ParticipantDTO>();
                        List<ParticipantDTO> blueTeam = new List<ParticipantDTO>();
                        MatchDTO matchData = match_V5.GetMatchData(arr[i]);
                        matchData.info.participants = InitParticipants(matchData.info.participants);
                        GetPerksImg(rune, matchData);
                        GetStatsImg(rune, matchData);
                        GetTeam(matchData, redTeam, blueTeam);
                        ParticipantDTO userData = match_V5.GetUserData(matchData);
                        innerItems.Add(new RecordListItemViewModel()
                        {
                            Assists = userData.assists,
                            Kills = userData.kills,
                            Deaths = userData.deaths,
                            KDA = userData.KDA,
                            Win = GetWinLose(userData.win),
                            QueueType = match_V5.GetQueueType(matchData.info.queueId),
                            GameDuration = GetGameTime(matchData.info.gameDuration),
                            ChampionPhoto = userData.championPhoto,
                            Summoner1Casts = match_V5.GetSpellName(userData.Summoner1Id),
                            Summoner2Casts = match_V5.GetSpellName(userData.Summoner2Id),
                            Item0 = match_V5.ReturnItemPhoto(userData.item0),
                            Item1 = match_V5.ReturnItemPhoto(userData.item1),
                            Item3 = match_V5.ReturnItemPhoto(userData.item2),
                            Item2 = match_V5.ReturnItemPhoto(userData.item3),
                            Item4 = match_V5.ReturnItemPhoto(userData.item4),
                            Item5 = match_V5.ReturnItemPhoto(userData.item5),
                            Item6 = match_V5.ReturnItemPhoto(userData.item6),
                            BlueTeam = blueTeam,
                            RedTeam = redTeam,
                            ChampionLevel = userData.champLevel,
                            TotalCS = userData.totalCs,
                            AvgCS = string.Format("{0:0.0}", userData.totalCs / ((double)matchData.info.gameDuration / 60)),
                            TotalDetecedWard = userData.detectorWardsPlaced,
                            TotalGold = string.Format("{0:#,###0}", userData.goldEarned),
                            KillRate = string.Format("{0:P0}", userData.killRate),
                            PrimaryPerks = userData.perks.styles[0].selections[0].perkImage,
                            SubPerks = userData.perks.styles[1].styleIcon,
                            gametime = matchData.info.gameDuration,
                            teams = matchData.info.teams,
                            Perks = userData.perks,
                        });
                    }
                }
                return items;
            }
        }*/
        #endregion

        private Prism.Commands.DelegateCommand buttonSearch;

        public ICommand ButtonSearch => buttonSearch = buttonSearch ?? new Prism.Commands.DelegateCommand(ButtonSearchCommand);

        private void ButtonSearchCommand()
        {
            Summoner_V4 summoner_V4 = new Summoner_V4();
            Summoner = summoner_V4.GetSummonerByName(SummonerName);
            Summoner.SummonerName = SummonerName;
            if (string.IsNullOrEmpty(SummonerName))
                return;
            if (Summoner != null)
            {
                League_V4 league_V4 = new League_V4();
                var position = league_V4.GetPosition(Summoner);
                SearchName = Summoner.SummonerName;
                SummonerLevel = Summoner.SummonerLevel;
                ProfileIconId = "http://opgg-static.akamaized.net/images/profile_icons/profileIcon" + Summoner.ProfileIconId + ".jpg";
                Tier = position.Tier + " " + position.Rank;
                Wins = position.Wins;
                Losses = position.Losses;
                TierIcon = "C:\\Users\\user\\source\\repos\\testlol\\testlol\\TierIcon\\Tier_" + position.Tier + ".png";
            }
            else
            {
                MessageBox.Show("Not Found");
            }

        }
    }
}
