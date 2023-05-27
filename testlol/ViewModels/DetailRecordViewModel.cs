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
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[i].teamId == 100) //블루
                {
                    BlueBaronKill = teams[i].objectives.baron.kills;
                    BlueDragonKill = teams[i].objectives.dragon.kills;
                    BlueTowerKill = teams[i].objectives.tower.kills;
                    BlueChampionKill = teams[i].objectives.champion.kills;
                    BlueInhibitorKill = teams[i].objectives.inhibitor.kills;
                    BlueRiftHeraldKill = teams[i].objectives.riftHerald.kills;
                    BlueTotalAssists = GetTotalAssists(blue);
                    BlueTotalDeaths = GetTotalDeaths(blue);
                }
                else if (teams[i].teamId == 200)
                {
                    RedBaronKill = teams[i].objectives.baron.kills;
                    RedDragonKill = teams[i].objectives.dragon.kills;
                    RedTowerKill = teams[i].objectives.tower.kills;
                    RedChampionKill = teams[i].objectives.champion.kills;
                    RedInhibitorKill = teams[i].objectives.inhibitor.kills;
                    RedRiftHeraldKill = teams[i].objectives.riftHerald.kills;
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
                    redTeam = new ReadOnlyObservableCollection<DetailListItemViewModel>(innerRed);
                    Match_V5 match_V5 = new Match_V5();
                    for (int i = 0; i < 5; i++)
                    {
                        innerRed.Add(new DetailListItemViewModel()
                        { 
                            tier = Red[i].tier,
                            ChampionLevel = Red[i].champLevel,
                            Summoner1Casts = match_V5.GetSpellName(Red[i].Summoner1Id),
                            Summoner2Casts = match_V5.GetSpellName(Red[i].Summoner2Id),
                            PrimaryPerks= Red[i].perks.styles[0].selections[0].perkImage,
                            SubPerks = Red[i].perks.styles[1].styleIcon,
                            Item0 = match_V5.ReturnItemPhoto(Red[i].item0),
                            Item1 = match_V5.ReturnItemPhoto(Red[i].item1),
                            Item2 = match_V5.ReturnItemPhoto(Red[i].item2),
                            Item3 = match_V5.ReturnItemPhoto(Red[i].item3),
                            Item4 = match_V5.ReturnItemPhoto(Red[i].item4),
                            Item5 = match_V5.ReturnItemPhoto(Red[i].item5),
                            Item6 = match_V5.ReturnItemPhoto(Red[i].item6),
                            SummonerName = Red[i].summonerName,
                            ChampionName = Red[i].championPhoto,
                            KDA = Red[i].KDA,
                            TotalDamgeTaken = string.Format("{0:#,###0}", Red[i].totalDamageTaken),
                            TotalDamge = string.Format("{0:#,###0}", Red[i].totalDamageDealtToChampions),
                            MaxDamge = GetMaxDamge(Red,Blue),
                            MaxDamgeTaken = GetMaxDamgeTaken(Red,Blue),
                            TotalCS = Red[i].totalCs,
                            AvgCS = string.Format("{0:0.0}", Red[i].totalCs / ((double)GameDuration / 60)),
                            TotalGold = string.Format("{0:#,###0G}", Red[i].goldEarned),
                            visionSocreAndDetectedWards = Red[i].detectorWardsPlaced+" / " + Red[i].visionScore,
                            wardPslacedandkilled = Red[i].wardsPlaced + " / "+ Red[i].wardsKilled,
                            kdaScore = Red[i].kills + " / "+ Red[i].deaths+" / "+ Red[i].assists+"("+ String.Format("{0:P0}",Red[i].killRate)+")",
                        });
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
                    blueTeam = new ReadOnlyObservableCollection<DetailListItemViewModel>(innerBlue);
                    Match_V5 match_V5 = new Match_V5();
                    for (int i = 0; i < 5; i++)
                    {
                        innerBlue.Add(new DetailListItemViewModel()
                        {
                            tier = Blue[i].tier,
                            ChampionLevel = Blue[i].champLevel,
                            Summoner1Casts = match_V5.GetSpellName(Blue[i].Summoner1Id),
                            Summoner2Casts = match_V5.GetSpellName(Blue[i].Summoner2Id),
                            PrimaryPerks = Blue[i].perks.styles[0].selections[0].perkImage,
                            SubPerks = Blue[i].perks.styles[1].styleIcon,
                            Item0 = match_V5.ReturnItemPhoto(Blue[i].item0),
                            Item1 = match_V5.ReturnItemPhoto(Blue[i].item1),
                            Item2 = match_V5.ReturnItemPhoto(Blue[i].item2),
                            Item3 = match_V5.ReturnItemPhoto(Blue[i].item3),
                            Item4 = match_V5.ReturnItemPhoto(Blue[i].item4),
                            Item5 = match_V5.ReturnItemPhoto(Blue[i].item5),
                            Item6 = match_V5.ReturnItemPhoto(Blue[i].item6),
                            SummonerName = Blue[i].summonerName,
                            ChampionName = Blue[i].championPhoto,
                            KDA = Blue[i].KDA,
                            TotalDamgeTaken = string.Format("{0:#,###0}", Blue[i].totalDamageTaken),
                            TotalDamge = string.Format("{0:#,###0}", Blue[i].totalDamageDealtToChampions),
                            MaxDamge = GetMaxDamge(Red, Blue),
                            MaxDamgeTaken = GetMaxDamgeTaken(Red, Blue),
                            TotalCS = Blue[i].totalCs,
                            AvgCS = string.Format("{0:0.0}", Blue[i].totalCs / ((double)GameDuration / 60)),
                            TotalGold = string.Format("{0:#,###0G}", Blue[i].goldEarned),
                            visionSocreAndDetectedWards = Blue[i].detectorWardsPlaced + " / " + Blue[i].visionScore,
                            wardPslacedandkilled = Blue[i].wardsPlaced + " / " + Blue[i].wardsKilled,
                            kdaScore = Blue[i].kills + " / " + Blue[i].deaths + " / " + Blue[i].assists + "(" + String.Format("{0:P0}", Blue[i].killRate) + ")",
                        });
                    }

                }
                return blueTeam;
            }
        }
        private int GetMaxDamge(List<ParticipantDTO> red, List<ParticipantDTO> blue)
        {
            int maxDamge = 0;
            for (int i = 0; i < red.Count; i++)
            {
                if (maxDamge <= red[i].totalDamageDealtToChampions)
                    maxDamge = red[i].totalDamageDealtToChampions;
                if (maxDamge <= blue[i].totalDamageDealtToChampions)
                    maxDamge = blue[i].totalDamageDealtToChampions;
            }
            return maxDamge;
        }
        private int GetMaxDamgeTaken(List<ParticipantDTO> red, List<ParticipantDTO> blue)
        {
            int maxDamge = 0;
            for (int i = 0; i < red.Count; i++)
            {
                if (maxDamge <= red[i].totalDamageTaken)
                    maxDamge = red[i].totalDamageTaken;
                if (maxDamge <= blue[i].totalDamageTaken)
                    maxDamge = blue[i].totalDamageTaken;
            }
            return maxDamge;
        }
        private int GetTotalDeaths(List<ParticipantDTO> team)
        {
            int Total = 0;
            for (int i = 0; i < team.Count; i++)
            {
                Total = Total + team[i].deaths;
            }
            return Total;
        }
        private int GetTotalAssists(List<ParticipantDTO> team)
        {
            int Total = 0;
            for (int i = 0; i < team.Count; i++)
            {
                Total = Total + team[i].assists;
            }
            return Total;
        }
    }
}
