using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using testlol.API;
using testlol.Managers;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Spectator_V4;
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class QueueViewModel : ViewModelBase
    {
        public QueueViewModel()
        {
            Spectator_V4 spectator_V4 = new Spectator_V4();
            GameInfo = spectator_V4.GetCurrentGameInfo(UserDataManager.Instance.Summoner.Id);
            if (GameInfo == null)
            {
                Visible = Visibility.Hidden;
                GameMode = "현재 게임중이 아닙니다.";
            }
            else
            {
                Visible = Visibility.Visible;
                Match_V5 match = new Match_V5();
                GameMode = match.GetQueueType((int)GameInfo.gameQueueConfigId);
            }
        }
        private Visibility visible;
        public Visibility Visible
        {
            get => visible;
            set => SetProperty(ref visible, value);
        }

        private CurrentGameInfoDTO gameInfo;
        public CurrentGameInfoDTO GameInfo
        {
            get => gameInfo;
            set => SetProperty(ref gameInfo, value);
        }
        private string gameMode;
        public string GameMode
        {
            get => gameMode;
            set => SetProperty(ref gameMode, value);
        }
        private ObservableCollection<QueueItemViewModel> innerRed
        { get; } = new ObservableCollection<QueueItemViewModel>();

        private ReadOnlyObservableCollection<QueueItemViewModel> redTeam;
        public ReadOnlyObservableCollection<QueueItemViewModel> RedTeam
        {
            get
            {

                if (redTeam == null && GameInfo != null && UserDataManager.Instance.Summoner != null)
                {
                    redTeam = new ReadOnlyObservableCollection<QueueItemViewModel>(innerRed);
                    List<CurrentGameParticipantDTO> participants = new List<CurrentGameParticipantDTO>();
                    List<CurrentGameParticipantDTO> red = new List<CurrentGameParticipantDTO>();
                    League_V4 league = new League_V4();
                    Match_V5 match_V5 = new Match_V5();
                    Spectator_V4 spectator_V4 = new Spectator_V4();
                    ChampionsDTO champions = spectator_V4.GetChampions();
                    participants = GameInfo.participants;
                    List<RuneDTO> rune = match_V5.GetRune();
                    string compare = "red";
                    foreach (var item in participants)
                    {
                        if (item.teamid == 200)
                            red.Add(item);
                    }

                    foreach (var item in red)
                    {
                        var position = league.GetPositions(item.summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
                        if (position == null)
                            position.Tier = "UnRanked";

                        foreach (var data in champions.data) // 챔피언 아이디 값으로 이름 찾기
                        {
                            if (long.Parse(champions.data[data.Key].key) == item.championid)
                            {
                                item.championName = champions.data[data.Key].id;
                            }

                        }
                        match_V5.GetPerks(item.perks, rune);
                        innerRed.Add(QueueItemViewModel.From(compare, position, league, item));
                    }

                }
                return redTeam;
            }
        }
        private ObservableCollection<QueueItemViewModel> innerBlue
        { get; } = new ObservableCollection<QueueItemViewModel>();

        private ReadOnlyObservableCollection<QueueItemViewModel> blueTeam;
        public ReadOnlyObservableCollection<QueueItemViewModel> BlueTeam
        {
            get
            {

                if (blueTeam == null && GameInfo != null && UserDataManager.Instance.Summoner != null)
                {
                    blueTeam = new ReadOnlyObservableCollection<QueueItemViewModel>(innerBlue);
                    List<CurrentGameParticipantDTO> participants = new List<CurrentGameParticipantDTO>();
                    List<CurrentGameParticipantDTO> blue = new List<CurrentGameParticipantDTO>();
                    League_V4 league = new League_V4();
                    Match_V5 match_V5 = new Match_V5();
                    Spectator_V4 spectator_V4 = new Spectator_V4();
                    ChampionsDTO champions = spectator_V4.GetChampions();
                    participants = GameInfo.participants;
                    List<RuneDTO> rune = match_V5.GetRune();
                    string compare = "blue";
                    foreach (var item in participants)
                    {
                        if (item.teamid == 100)
                            blue.Add(item);
                    }

                    foreach (var item in blue)
                    {
                        var position = league.GetPositions(item.summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
                        if (position == null)
                            position.Tier = "UnRanked";
                        foreach (var data in champions.data)
                        {
                            if (long.Parse(champions.data[data.Key].key) == item.championid)
                            {
                                item.championName = champions.data[data.Key].id;
                            }

                        }

                        match_V5.GetPerks(item.perks, rune);

                        innerBlue.Add(QueueItemViewModel.From(compare, position, league, item));
                    }

                }
                return blueTeam;
            }
        }

    }
}
