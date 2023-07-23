using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.API;
using testlol.Models.DTOs.Match_V5;

namespace testlol.ViewModels
{
    internal class DetailRecordViewModel : ViewModelBase
    {
        public DetailRecordViewModel()
        {

        }
        public DetailRecordViewModel(List<ParticipantDTO> red, List<ParticipantDTO> blue, long gametime, List<TeamDTO> teams)
        {
            Red = red;
            Blue = blue;
            GameDuration = gametime;

            foreach (var item in teams)
            {
                if (item.teamId == 100) //블루
                {
                    BlueBaronKill = item.objectives.baron.kills;
                    BlueDragonKill = item.objectives.dragon.kills;
                    BlueTowerKill = item.objectives.tower.kills;
                    BlueChampionKill = item.objectives.champion.kills;
                    BlueInhibitorKill = item.objectives.inhibitor.kills;
                    BlueRiftHeraldKill = item.objectives.riftHerald.kills;
                    BlueTotalAssists = GetTotalAssists(blue);
                    BlueTotalDeaths = GetTotalDeaths(blue);
                }
                else if (item.teamId == 200)
                {
                    RedBaronKill = item.objectives.baron.kills;
                    RedDragonKill = item.objectives.dragon.kills;
                    RedTowerKill = item.objectives.tower.kills;
                    RedChampionKill = item.objectives.champion.kills;
                    RedInhibitorKill = item.objectives.inhibitor.kills;
                    RedRiftHeraldKill = item.objectives.riftHerald.kills;
                    RedTotalAssists = GetTotalAssists(red);
                    RedTotalDeaths = GetTotalDeaths(red);
                }
            }
            if (red[1].win == true)
            {
                RedWin = "레드팀 승리";
                BlueWin = "블루팀 패배";
            }else if(blue[1].win == true)
            {
                RedWin = "레드팀 패배";
                BlueWin = "블루팀 승리";
            }
        }

        #region properties
        private string redWin;
        public string RedWin
        {
            get => redWin;
            set => SetProperty(ref redWin, value);
        }
        private string blueWin;
        public string BlueWin
        {
            get => blueWin;
            set => SetProperty(ref blueWin, value);
        }
        private long gameduration;
        public long GameDuration
        {
            get => gameduration;
            set => SetProperty(ref gameduration, value);
        }
        private int redBaronKill;
        public int RedBaronKill
        {
            get => redBaronKill;
            set => SetProperty(ref redBaronKill, value);
        }

        private int blueBaronKill;
        public int BlueBaronKill
        {
            get => blueBaronKill;
            set => SetProperty(ref blueBaronKill, value);
        }
        private int redDragonKill;
        public int RedDragonKill
        {
            get => redDragonKill;
            set => SetProperty(ref redDragonKill, value);
        }
        private int blueDragonKill;
        public int BlueDragonKill
        {
            get => blueDragonKill;
            set => SetProperty(ref blueDragonKill, value);
        }
        private int redTowerKill;
        public int RedTowerKill
        {
            get => redTowerKill;
            set => SetProperty(ref redTowerKill, value);
        }
        private int blueTowerKill;
        public int BlueTowerKill
        {
            get => blueTowerKill;
            set => SetProperty(ref blueTowerKill, value);
        }
        private int redInhibitorKill;
        public int RedInhibitorKill
        {
            get => redInhibitorKill;
            set => SetProperty(ref redInhibitorKill, value);
        }
        private int blueInhibitorKill;
        public int BlueInhibitorKill
        {
            get => blueInhibitorKill;
            set => SetProperty(ref blueInhibitorKill, value);
        }
        private int redRiftHeraldKill;
        public int RedRiftHeraldKill
        {
            get => redRiftHeraldKill;
            set => SetProperty(ref redRiftHeraldKill, value);
        }
        private int blueRiftHeraldKill;
        public int BlueRiftHeraldKill
        {
            get => blueRiftHeraldKill;
            set => SetProperty(ref blueRiftHeraldKill, value);
        }
        private int redChampionKill;
        public int RedChampionKill
        {
            get => redChampionKill;
            set => SetProperty(ref redChampionKill, value);
        }
        private int blueChampionKill;
        public int BlueChampionKill
        {
            get => blueChampionKill;
            set => SetProperty(ref blueChampionKill, value);
        }
        private int redTotalDeaths;
        public int RedTotalDeaths
        {
            get => redTotalDeaths;
            set => SetProperty(ref redTotalDeaths, value);
        }
        private int blueTotalDeaths;
        public int BlueTotalDeaths
        {
            get => blueTotalDeaths;
            set => SetProperty(ref blueTotalDeaths, value);
        }
        private int redTotalAssists;
        public int RedTotalAssists
        {
            get => redTotalAssists;
            set => SetProperty(ref redTotalAssists, value);
        }
        private int blueTotalAssists;
        public int BlueTotalAssists
        {
            get => blueTotalAssists;
            set => SetProperty(ref blueTotalAssists, value);
        }


        private List<ParticipantDTO> red;
        public List<ParticipantDTO> Red
        {
            get => red;
            set => SetProperty(ref red, value);
        }

        private List<ParticipantDTO> blue;
        public List<ParticipantDTO> Blue
        {
            get => blue;
            set => SetProperty(ref blue, value);
        }
        #endregion
        
       
        private ObservableCollection<DetailListItemViewModel> innerRed
        { get; } = new ObservableCollection<DetailListItemViewModel>();

        private ReadOnlyObservableCollection<DetailListItemViewModel> redTeam;
        public ReadOnlyObservableCollection<DetailListItemViewModel> RedTeam
        {
            get
            {
                
                if (redTeam == null && red!=null)
                {
                    string compare = "red";
                    redTeam = new ReadOnlyObservableCollection<DetailListItemViewModel>(innerRed);
                    Match_V5 match_V5 = new Match_V5();
                    int maxDamge = GetMaxDamge(Red, Blue);
                    int maxDamgeTaken = GetMaxDamgeTaken(Red, Blue);
                    for (int i = 0; i < 5; i++)
                    {
                        innerRed.Add(DetailListItemViewModel.From(compare ,match_V5, Red, Blue, i, GameDuration, maxDamge, maxDamgeTaken));
                    }
                    
                }
                return redTeam;
            }
        }

        private ObservableCollection<DetailListItemViewModel> innerBlue
        { get; } = new ObservableCollection<DetailListItemViewModel>();

        private ReadOnlyObservableCollection<DetailListItemViewModel> blueTeam;
        public ReadOnlyObservableCollection<DetailListItemViewModel> BlueTeam
        {
            get
            {

                if (blueTeam == null && blue != null)
                {
                    string compare = "blue";
                    blueTeam = new ReadOnlyObservableCollection<DetailListItemViewModel>(innerBlue);
                    Match_V5 match_V5 = new Match_V5();
                    int maxDamge = GetMaxDamge(Red, Blue);
                    int maxDamgeTaken = GetMaxDamgeTaken(Red, Blue);
                    for (int i = 0; i < 5; i++)
                    {
                        innerBlue.Add(DetailListItemViewModel.From(compare, match_V5, Red, Blue, i, GameDuration, maxDamge, maxDamgeTaken));
                    }

                }
                return blueTeam;
            }
        }

        #region func
        private int GetMaxDamge(List<ParticipantDTO> red, List<ParticipantDTO> blue)
        {
            int maxDamge = 0;
            foreach (var item in red)
            {
                if (maxDamge <= item.totalDamageDealtToChampions)
                    maxDamge = item.totalDamageDealtToChampions;

            }
            foreach (var item in blue)
            {
                if (maxDamge <= item.totalDamageDealtToChampions)
                    maxDamge = item.totalDamageDealtToChampions;
            }
            return maxDamge;
        }
        private int GetMaxDamgeTaken(List<ParticipantDTO> red, List<ParticipantDTO> blue)
        {
            int maxDamge = 0;
            foreach (var item in red)
            {
                if (maxDamge <= item.totalDamageTaken)
                    maxDamge = item.totalDamageTaken;

            }
            foreach (var item in blue)
            {
                if (maxDamge <= item.totalDamageTaken)
                    maxDamge = item.totalDamageTaken;
            }
            return maxDamge;
        }
        private int GetTotalDeaths(List<ParticipantDTO> team)
        {
            int Total = 0;
            foreach (var item in team)
            {
                Total = Total + item.deaths;
            }
            return Total;
        }
        private int GetTotalAssists(List<ParticipantDTO> team)
        {
            int Total = 0;
            foreach (var item in team)
            {
                Total = Total + item.assists;
            }
            return Total;
        } 
        #endregion
    }
}
