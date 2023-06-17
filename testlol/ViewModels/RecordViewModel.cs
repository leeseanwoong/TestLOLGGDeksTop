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
using testlol.Models.DTOs.Match_V5;
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
            Match_V5 match_V5 = new Match_V5();
            string matchlist = match_V5.GetMatchList(Constants.Summoner.puuid);
            matchlist = matchlist.Replace("\"", "");
            matchlist = matchlist.Replace("[", "");
            matchlist = matchlist.Replace("]", "");
            string[] arr = matchlist.Split(",");
            int totalGameCount = arr.Length;
            int totalKills = 0;
            int totalDeaths = 0;
            int totalAssists = 0;
            int totalWinCount = 0;
            int totalLossesCount = 0;
            List<MostChampionDTO> mostChampions = new List<MostChampionDTO>();
            
            for (int i = 0; i < arr.Length; i++)
            {
                MostChampionDTO most = new MostChampionDTO();
                MatchDTO matchData = match_V5.GetMatchData(arr[i]);
                matchData.info.participants = match_V5.InitParticipants(matchData.info.participants);
                ParticipantDTO userData = match_V5.GetUserData(matchData,Constants.UserName);
                if (userData.win == true)
                {
                    totalWinCount = totalWinCount + 1;
                    most.WinCount +=1;
                }
                else if (userData.win == false)
                {
                    totalLossesCount = totalLossesCount + 1;
                    most.LossesCount+=1;
                }
                most.ChampionPhoto = userData.championPhoto;
                most.ChampionName = userData.championName;
                most.ChampCount = most.ChampCount + 1;
                most.Kills = userData.kills;
                most.Deaths = userData.deaths;
                most.Assists = userData.assists;
                totalKills  = totalKills + userData.kills;
                totalDeaths =  totalDeaths + userData.deaths;
                totalAssists =  totalAssists + userData.assists;
                most.KDA = Math.Round(((double)most.Kills + most.Assists) / most.Deaths, 2);
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
            var mostList = mostChampions.OrderByDescending(x => x.ChampCount).ToList();
            TotalGameCount = totalGameCount + " 전 " + totalWinCount + " 승 " + totalLossesCount + " 패";
            TotalWinRate = String.Format("{0:P0}", (double)totalWinCount / totalGameCount);
            TotalKDAScore = string.Format("{0:#,###0.#}", (double)totalKills / 10) + " / " + string.Format("{0:#,###0.#}", (double)totalAssists / 10) + " / " + string.Format("{0:#,###0.#}", (double)totalDeaths / 10);
            TotalKDA = String.Format("{0:0.0#}", (double)(totalKills + totalAssists) / totalDeaths);
            MostChampions = mostList;
        }
        #endregion

        #region property
        private ObservableCollection<RecordListItemViewModel> innermembers
        { get; } = new ObservableCollection<RecordListItemViewModel>();

        private ReadOnlyObservableCollection<RecordListItemViewModel> members;

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

        public ReadOnlyObservableCollection<RecordListItemViewModel> Members
        {
            get
            {
                if (members == null)
                {
                    members = new ReadOnlyObservableCollection<RecordListItemViewModel>(innermembers);
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
                        matchData.info.participants = match_V5.InitParticipants(matchData.info.participants);
                        match_V5.GetPerksImg(rune, matchData);
                        match_V5.GetStatsImg(rune, matchData);
                        match_V5.GetTeam(matchData, redTeam, blueTeam);
                        ParticipantDTO userData = match_V5.GetUserData(matchData,Constants.UserName);
                        innermembers.Add(new RecordListItemViewModel()
                        {
                            Assists = userData.assists,
                            Kills = userData.kills,
                            Deaths = userData.deaths,
                            KDA = userData.KDA,
                            Win = match_V5.GetWinLose(userData.win),
                            QueueType = match_V5.GetQueueType(matchData.info.queueId),
                            GameDuration = match_V5.GetGameTime(matchData.info.gameDuration),
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
                return members;
            }
        }

        #endregion

        #region ButtonEvent
        private Utills.DelegateCommand buttonDetailPopup;
        public ICommand ButtonDetailPopup => buttonDetailPopup ?? new Utills.DelegateCommand(ButtonDeatilPopupCommand, CanButtonClick);

        private void ButtonDeatilPopupCommand(object parameter)
        {
            int idx = Members.IndexOf(parameter as RecordListItemViewModel);
            if (idx > -1 && idx < Members.Count)
            {
                GetTier(Members[idx].RedTeam);
                GetTier(Members[idx].BlueTeam);

                var viewModel = new DetailRecordViewModel(Members[idx].RedTeam, Members[idx].BlueTeam, Members[idx].gametime, Members[idx].teams);
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
            int idx = Members.IndexOf(parameter as RecordListItemViewModel);
            if (idx > -1 && idx < Members.Count)
            {
                var viewModel = new PerksPopUpViewModel(Members[idx].Perks);
                PerksPopUpView perksView = new PerksPopUpView();
                perksView.DataContext = viewModel;

                perksView.Show();
            }

        }
        #endregion

        #region functions
        private void GetTier(List<ParticipantDTO> team)
        {
            League_V4 league = new League_V4();


            for (int i = 0; i < team.Count; i++)
            {
                var position = league.GetPositions(team[i].summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
                if (position == null)
                    team[i].tier = "Unranked";
                else
                    team[i].tier = position.Tier + " " + position.Rank;
            }
        }
        #endregion


    }
}
