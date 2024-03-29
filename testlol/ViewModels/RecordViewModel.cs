﻿using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testlol.API;
using testlol.Managers;
using testlol.Models.DTOs;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Spectator_V4;
using testlol.Types;
using testlol.Utills;
using testlol.Views;

namespace testlol.ViewModels
{
    internal class RecordViewModel : ViewModelBase
    {
        #region ctor
        public RecordViewModel()
        {
            InitializeAsync();
        }
        #endregion
        private async void InitializeAsync()
        {
            IsLoading = true;
            await InitMostAsync();
            await LoadMatchListAsync();
            IsLoading = false;
        }
        private async Task InitMostAsync()
        {
            Match_V5 match_V5 = new Match_V5();
            var matchlist = match_V5.GetMatchList(UserDataManager.Instance.Summoner.puuid);


            int totalGameCount = matchlist.Count();
            int totalKills = 0;
            int totalDeaths = 0;
            int totalAssists = 0;
            int totalWinCount = 0;
            int totalLossesCount = 0;
            List<MostChampionDTO> mostChampions = new List<MostChampionDTO>();

            await Task.Run(() =>
            {
                foreach (var item in matchlist)
                {
                    MostChampionDTO most = new MostChampionDTO();
                    MatchDTO matchData = match_V5.GetMatchData(item);
                    matchData.info.participants = LoLUtility.InitParticipants(matchData.info.participants);
                    ParticipantDTO userData = LoLUtility.GetUserData(matchData, UserDataManager.Instance.Summoner.Name);
                    if (userData.win == true)
                    {
                        totalWinCount = totalWinCount + 1;
                        most.WinCount += 1;
                    }
                    else if (userData.win == false)
                    {
                        totalLossesCount = totalLossesCount + 1;
                        most.LossesCount += 1;
                    }
                    most.ChampionPhoto = userData.championPhoto;
                    most.ChampionName = userData.championName;
                    most.ChampCount = most.ChampCount + 1;
                    most.Kills = userData.kills;
                    most.Deaths = userData.deaths;
                    most.Assists = userData.assists;
                    totalKills = totalKills + userData.kills;
                    totalDeaths = totalDeaths + userData.deaths;
                    totalAssists = totalAssists + userData.assists;
                    most.KDA = (most.Deaths == 0) ? Math.Round((double)most.Kills + most.Assists, 2) : Math.Round(((double)most.Kills + most.Assists) / most.Deaths, 2);
                    mostChampions.Add(most);
                }

                for (int j = 0; j < mostChampions.Count; j++)
                {
                    for (int k = 0; k < mostChampions.Count; k++)
                    {
                        if (j == k) { }
                        else
                        {
                            if (mostChampions[j].ChampionName == mostChampions[k].ChampionName)
                            {
                                mostChampions[j].Assists += mostChampions[k].Assists;
                                mostChampions[j].Deaths += mostChampions[k].Deaths;
                                mostChampions[j].Kills += mostChampions[k].Kills;
                                mostChampions[j].LossesCount += mostChampions[k].LossesCount;
                                mostChampions[j].WinCount += mostChampions[k].WinCount;
                                mostChampions[j].ChampCount += 1;
                                mostChampions[j].KDA = Math.Round(((double)mostChampions[j].Kills + mostChampions[j].Assists) / mostChampions[j].Deaths, 2);
                                mostChampions.RemoveAt(k);
                                k += -1;
                            }
                        }
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mostList = mostChampions.OrderByDescending(x => x.ChampCount).ToList();
                    TotalGameCount = totalGameCount + " 전 " + totalWinCount + " 승 " + totalLossesCount + " 패";
                    TotalWinRate = String.Format("{0:P0}", (double)totalWinCount / totalGameCount);
                    TotalKDAScore = string.Format("{0:#,###0.#}", (double)totalKills / 10) + " / " + string.Format("{0:#,###0.#}", (double)totalAssists / 10) + " / " + string.Format("{0:#,###0.#}", (double)totalDeaths / 10);
                    TotalKDA = String.Format("{0:0.0#}", (double)(totalKills + totalAssists) / totalDeaths);
                    MostChampions = mostList;
                });
            });


        }
        private async Task LoadMatchListAsync()
        {


            Match_V5 match_V5 = new Match_V5();
            var matchlist = match_V5.GetMatchList(UserDataManager.Instance.Summoner.puuid);

            foreach (var item in matchlist)
            {
                Application.Current.Dispatcher.Invoke(() => // UI 스레드에서 실행
                {
                    innermembers.Add(RecordListItemViewModel.From(item));
                });
            }
        }
        #region property


        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        private List<MostChampionDTO> mostChampions;
        public List<MostChampionDTO> MostChampions
        {
            get => mostChampions;
            set => SetProperty(ref mostChampions, value);
        }
        private string totalWinRate;
        public string TotalWinRate
        {
            get => totalWinRate;
            set => SetProperty(ref totalWinRate, value);
        }
        private string totalGameCount;
        public string TotalGameCount
        {
            get => totalGameCount;
            set => SetProperty(ref totalGameCount, value);
        }
        private string totalKDA;
        public string TotalKDA
        {
            get => totalKDA;
            set => SetProperty(ref totalKDA, value);
        }
        private string totalKDAScore;
        public string TotalKDAScore
        {
            get => totalKDAScore;
            set => SetProperty(ref totalKDAScore, value);
        }
        private ObservableCollection<RecordListItemViewModel> innermembers
        { get; } = new ObservableCollection<RecordListItemViewModel>();

        private ReadOnlyObservableCollection<RecordListItemViewModel> members;

        public ReadOnlyObservableCollection<RecordListItemViewModel> Members
        {
            get
            {
                if (members == null && UserDataManager.Instance.Summoner != null)
                {
                    members = new ReadOnlyObservableCollection<RecordListItemViewModel>(innermembers);
                    //LoadMatchListAsync();
                }
                return members;
            }
        }

        

        #endregion

        #region ButtonEvent
        private Utills.DelegateCommand buttonDetailPopup;
        public ICommand ButtonDetailPopup => buttonDetailPopup ?? new Utills.DelegateCommand(ButtonDeatilPopupCommand, CanButtonClick);

        private void ButtonDeatilPopupCommand(object parameter)
        {
            Match_V5 match_V5 = new Match_V5();
            int idx = Members.IndexOf(parameter as RecordListItemViewModel);
            if (idx > -1 && idx < Members.Count)
            {
                if (Members[idx].QueueType == "아레나")
                {
                    var viewModel = new ArenaDetailViewModel(Members[idx].ArenaTeams, Members[idx].gametime);
                    ArenaDetailView arenaDetailView = new ArenaDetailView();
                    arenaDetailView.DataContext = viewModel;

                    arenaDetailView.Show();
                }
                else
                {
                    LoLUtility.GetTier(Members[idx].RedTeam);
                    LoLUtility.GetTier(Members[idx].BlueTeam);

                    var viewModel = new DetailRecordViewModel(Members[idx].RedTeam, Members[idx].BlueTeam, Members[idx].gametime, Members[idx].teams);
                    DetailRecordView detailRecordView = new DetailRecordView();
                    detailRecordView.DataContext = viewModel;

                    detailRecordView.Show();
                }
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
            int idx = Members.IndexOf(parameter as RecordListItemViewModel);
            if (idx > -1 && idx < Members.Count)
            {
                var viewModel = new PerksPopUpViewModel(Members[idx].Perks);
                PerksPopUpView perksView = new PerksPopUpView();
                perksView.DataContext = viewModel;

                perksView.Show();
            }

        }

        public Prism.Commands.DelegateCommand buttonInGamePopUp;
        public ICommand ButtonInGamePopUp => buttonInGamePopUp = buttonInGamePopUp ?? new Prism.Commands.DelegateCommand(ButtonInGamePopUpCommand);

        private void ButtonInGamePopUpCommand()
        {
            Spectator_V4 spectator_V4 = new Spectator_V4();
            CurrentGameInfoDTO GameInfo = spectator_V4.GetCurrentGameInfo(UserDataManager.Instance.Summoner.Id);
            if (GameInfo == null)
            {
                MessageBox.Show(UserDataManager.Instance.Summoner.Name + " 님은 게임 중이 아닙니다.");
            }
            else
            {
                var viewModel = new QueueViewModel();
                QueueView queueView = new QueueView();
                queueView.DataContext = viewModel;

                queueView.Show();
            }
        }
        #endregion



    }
}
