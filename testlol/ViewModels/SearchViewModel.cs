﻿using System;
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
using System.ComponentModel;
using System.Windows.Data;
using testlol.Views;
using testlol.Models.DTOs.Spectator_V4;
using static System.Net.WebRequestMethods;

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
        private string wins;
        public string Wins
        {
            get => wins;
            set => SetProperty(ref wins, value);
        }
        private string losses;
        public string Losses
        {
            get => losses;
            set => SetProperty(ref losses, value);
        }
        private string leaguePoints;
        public string LeaguePoints
        {
            get => leaguePoints;
            set => SetProperty(ref leaguePoints, value);
        }
        private string winRate;
        public string WinRate
        {
            get => winRate;
            set => SetProperty(ref winRate, value);
        }

        private SummonerDTO summoner;
        public SummonerDTO Summoner
        {
            get => summoner;
            set => SetProperty(ref summoner, value);
        }

        private ObservableCollection<RecordListItemViewModel> items = new ObservableCollection<RecordListItemViewModel>();
        public ObservableCollection<RecordListItemViewModel> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        #endregion

        #region Button
        private Prism.Commands.DelegateCommand buttonSearch;

        public ICommand ButtonSearch => buttonSearch = buttonSearch ?? new Prism.Commands.DelegateCommand(ButtonSearchCommand);

        private void ButtonSearchCommand()
        {
            Summoner_V4 summoner_V4 = new Summoner_V4();
            League_V4 league_V4 = new League_V4();
            Match_V5 match_V5 = new Match_V5();
            Summoner = summoner_V4.GetSummonerByName(SummonerName);
            if (string.IsNullOrEmpty(SummonerName))
                return;
            if (Summoner != null)
            {
                items.Clear();
                var position = league_V4.GetPosition(Summoner);
                SearchName = Summoner.Name;
                SummonerLevel = Summoner.SummonerLevel;
                ProfileIconId = "http://opgg-static.akamaized.net/images/profile_icons/profileIcon" + Summoner.ProfileIconId + ".jpg";
                if (position.Tier == null)
                {
                    Tier = "UnRanked";
                    TierIcon = "https://z.fow.kr/img/emblem/unranked.png";
                    WinRate = null;
                }
                else
                {
                    Tier = position.Tier + " " + position.Rank;
                    TierIcon = "https://z.fow.kr/img/emblem/" + position.Tier.ToLower() + ".png";
                    LeaguePoints = position.leaguePoints + " LP";
                    WinRate = "(" + string.Format("{0:P0}", (double)position.Wins / (position.Wins + position.Losses)) + ")";
                }

                Wins = position.Wins + "승 ";
                Losses = position.Losses + "패 ";


                var matchlist = match_V5.GetMatchList(Summoner.puuid);
                List<RuneDTO> rune = match_V5.GetRune();
                foreach (var item in matchlist)
                {
                    List<ParticipantDTO> redTeam = new List<ParticipantDTO>();
                    List<ParticipantDTO> blueTeam = new List<ParticipantDTO>();
                    MatchDTO matchData = match_V5.GetMatchData(item);
                    matchData.info.participants = match_V5.InitParticipants(matchData.info.participants);
                    match_V5.GetPerksImg(rune, matchData);
                    match_V5.GetStatsImg(rune, matchData);
                    match_V5.GetTeam(matchData, redTeam, blueTeam);
                    ParticipantDTO userData = match_V5.GetUserData(matchData, Summoner.Name);
                    Items.Add(RecordListItemViewModel.From(match_V5, userData, matchData, redTeam, blueTeam));
                }

            }
            else
            {
                MessageBox.Show("Not Found");
            }

        }

        private Prism.Commands.DelegateCommand buttonQueue;
        public ICommand ButtonQueue => buttonQueue = buttonQueue ?? new Prism.Commands.DelegateCommand(ButtonQueueCommand);
        private void ButtonQueueCommand()
        {
            Spectator_V4 spectator_V4 = new Spectator_V4();
            if (Summoner == null)
                MessageBox.Show("소환사를 검색해주세요.");
            else
            {
                CurrentGameInfoDTO GameInfo = spectator_V4.GetCurrentGameInfo(Summoner.Id);
                if (GameInfo == null)
                {
                    MessageBox.Show(Summoner.Name + " 은 현재 게임중이 아닙니다.");
                }
                else
                {
                    var viewModel = new SearchUserQueueViewModel(GameInfo);
                    SearchUserQueueView searchUserQueueView = new SearchUserQueueView();
                    searchUserQueueView.DataContext = viewModel;

                    searchUserQueueView.Show();
                }
            }
        }

        private Utills.DelegateCommand buttonDetailPopup;
        public ICommand ButtonDetailPopup => buttonDetailPopup ?? new Utills.DelegateCommand(ButtonDeatilPopupCommand, CanButtonClick);

        private void ButtonDeatilPopupCommand(object parameter)
        {
            Match_V5 match_V5 = new Match_V5();
            int idx = Items.IndexOf(parameter as RecordListItemViewModel);
            if (idx > -1 && idx < Items.Count)
            {
                match_V5.GetTier(Items[idx].RedTeam);
                match_V5.GetTier(Items[idx].BlueTeam);

                var viewModel = new DetailRecordViewModel(Items[idx].RedTeam, Items[idx].BlueTeam, Items[idx].gametime, Items[idx].teams);
                DetailRecordView detailRecordView = new DetailRecordView();
                detailRecordView.DataContext = viewModel;

                detailRecordView.Show();
            }

        }
        private bool CanButtonClick(object parameter)
        {
            return true;
        }

        private Utills.DelegateCommand buttonPerksPopup;
        public ICommand ButtonPerksPopup => buttonPerksPopup = buttonPerksPopup ?? new Utills.DelegateCommand(ButtonPerksPopupCommand, CanButtonClick);
        private void ButtonPerksPopupCommand(object parameter)
        {
            int idx = Items.IndexOf(parameter as RecordListItemViewModel);
            if (idx > -1 && idx < Items.Count)
            {
                var viewModel = new PerksPopUpViewModel(Items[idx].Perks);
                PerksPopUpView perksView = new PerksPopUpView();
                perksView.DataContext = viewModel;

                perksView.Show();
            }

        } 
        #endregion
    }
}
