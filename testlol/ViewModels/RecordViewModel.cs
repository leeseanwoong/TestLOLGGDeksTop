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
                matchData.info.participants = InitParticipants(matchData.info.participants);
                ParticipantDTO userData = match_V5.GetUserData(matchData);
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
                        matchData.info.participants = InitParticipants(matchData.info.participants);
                        GetPerksImg(rune, matchData);
                        GetStatsImg(rune, matchData);
                        GetTeam(matchData, redTeam, blueTeam);
                        ParticipantDTO userData = match_V5.GetUserData(matchData);
                        innermembers.Add(new RecordListItemViewModel()
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
                return members;
            }
        }

        #endregion

        #region ButtonEvent
        private DelegateCommand buttonDetailPopup;
        public ICommand ButtonDetailPopup => buttonDetailPopup ?? new DelegateCommand(ButtonDeatilPopupCommand, CanButtonClick);

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

        private DelegateCommand buttonPerksPopup;
        public ICommand ButtonPerksPopup => buttonPerksPopup = buttonPerksPopup ?? new DelegateCommand(ButtonPerksPopupCommand, CanButtonClick);
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
        private void GetTeam(MatchDTO matchdata, List<ParticipantDTO> redteam, List<ParticipantDTO> blueteam)
        {
            int bluetotalkill = 0;
            int redtotalkill = 0;
            for (int i = 0; i < matchdata.info.participants.Count; i++)
            {
                if (matchdata.info.participants[i].teamId == 100)
                {
                    blueteam.Add(matchdata.info.participants[i]);
                    bluetotalkill = bluetotalkill + matchdata.info.participants[i].kills;

                }
                else if (matchdata.info.participants[i].teamId == 200)
                {
                    redteam.Add(matchdata.info.participants[i]);
                    redtotalkill = redtotalkill + matchdata.info.participants[i].kills;
                }
            }
            for (int j = 0; j < matchdata.info.participants.Count; j++)
            {
                if (matchdata.info.participants[j].teamId == 100)
                {
                    for (int k = 0; k < blueteam.Count; k++)
                    {
                        if (matchdata.info.participants[j].summonerName == blueteam[k].summonerName)
                        {
                            matchdata.info.participants[j].killRate = (double)(matchdata.info.participants[j].kills + matchdata.info.participants[j].assists) / bluetotalkill;
                        }
                    }
                }
                else if (matchdata.info.participants[j].teamId == 200)
                {
                    for (int k = 0; k < redteam.Count; k++)
                    {
                        if (matchdata.info.participants[j].summonerName == redteam[k].summonerName)
                        {
                            matchdata.info.participants[j].killRate = (double)(matchdata.info.participants[j].kills + matchdata.info.participants[j].assists) / redtotalkill;
                        }
                    }
                }

            }
        }

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

        private string GetGameTime(long GameDuration)
        {
            TimeSpan t = TimeSpan.FromSeconds(GameDuration);
            return $"{t.Minutes}분 {t.Seconds}초";
        }


        private string GetWinLose(bool win)
        {
            if (win == true)
                return "승리";
            else
                return "패배";
        }
        private void GetPerksImg(List<RuneDTO> rune, MatchDTO matchDTO)
        {
            for (int i = 0; i < matchDTO.info.participants.Count; i++) //10번돔 - 멤버들전원
            {
                for (int j = 0; j < rune.Count; j++) // 룬개수만큼 5번
                {
                    for (int f = 0; f < matchDTO.info.participants[i].perks.styles.Count; f++) //2번 스타일 개수만큼
                    {
                        if (matchDTO.info.participants[i].perks.styles[f].style == rune[j].id)
                        {
                            matchDTO.info.participants[i].perks.styles[f].styleIcon = "https://ddragon.leagueoflegends.com/cdn/img/" + rune[j].icon; //스타일에 따른 이미지 가져옴 ex) 정밀, 영감 등
                            matchDTO.info.participants[i].perks.styles[f].styleName = rune[j].name;
                            for (int k = 0; k < matchDTO.info.participants[i].perks.styles[f].selections.Count; k++) // 4번, 2번 돌때 잇음
                            {
                                for (int t = 0; t < rune[j].slots.Count; t++)
                                {
                                    for (int e = 0; e < rune[j].slots[t].runes.Count; e++)
                                    {
                                        if (matchDTO.info.participants[i].perks.styles[f].selections[k].perk == rune[j].slots[t].runes[e].id)
                                        {
                                            matchDTO.info.participants[i].perks.styles[f].selections[k].perkImage = "https://ddragon.leagueoflegends.com/cdn/img/" + rune[j].slots[t].runes[e].icon;
                                            matchDTO.info.participants[i].perks.styles[f].selections[k].perkName = rune[j].slots[t].runes[e].name;
                                        }
                                    }

                                }

                            }
                        }
                    }

                }
            }
        }
        /*
         * HealthScaling = 5001, // 체력
        Armor = 5002, // 방어력
        MagicRes = 5003, // 마법저항력
        AttackSpeed = 5005, // 공격 : 속도
        CdrScaling = 5007, // 공격 : 쿨감
        Adaptive = 5008, // 적응형 능력치
        "defense": 5002, 맨 아래
                        "flex": 5008, 중앙
                        "offense": 5005 맨 위
         */
        private void GetStatsImg(List<RuneDTO> rune, MatchDTO matchDTO)
        {
            for (int i = 0; i < matchDTO.info.participants.Count; i++) //10번돔 - 멤버들전원
            {
                if (matchDTO.info.participants[i].perks.statPerks.offense == 5008) // 맨 위쪽
                {
                    matchDTO.info.participants[i].perks.statPerks.offenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png"; // 적응형 능력치
                    matchDTO.info.participants[i].perks.statPerks.offenseDesc = "적응형 능력치 + 9";
                }
                else if (matchDTO.info.participants[i].perks.statPerks.offense == 5005)
                {
                    matchDTO.info.participants[i].perks.statPerks.offenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsattackspeedicon.png";
                    matchDTO.info.participants[i].perks.statPerks.offenseDesc = "공격 속도 + 10%";
                }
                else if (matchDTO.info.participants[i].perks.statPerks.offense == 5007)
                {
                    matchDTO.info.participants[i].perks.statPerks.offenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodscdrscalingicon.png";
                    matchDTO.info.participants[i].perks.statPerks.offenseDesc = "스킬 가속 + 8";
                }

                if (matchDTO.info.participants[i].perks.statPerks.flex == 5008)
                {
                    matchDTO.info.participants[i].perks.statPerks.flexImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png";
                    matchDTO.info.participants[i].perks.statPerks.flexDesc = "적응형 능력치 + 9";
                }
                else if (matchDTO.info.participants[i].perks.statPerks.flex == 5002)
                {
                    matchDTO.info.participants[i].perks.statPerks.flexImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png";
                    matchDTO.info.participants[i].perks.statPerks.flexDesc = "방어력 +6";
                }
                else if (matchDTO.info.participants[i].perks.statPerks.flex == 5003)
                {
                    matchDTO.info.participants[i].perks.statPerks.flexImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png";
                    matchDTO.info.participants[i].perks.statPerks.flexDesc = "마법 저항력 +8";
                }

                if (matchDTO.info.participants[i].perks.statPerks.defense == 5001)
                {
                    matchDTO.info.participants[i].perks.statPerks.defenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodshealthscalingicon.png";
                    matchDTO.info.participants[i].perks.statPerks.defenseDesc = "체력 +15~140 (레벨에 비례)";
                }
                else if (matchDTO.info.participants[i].perks.statPerks.defense == 5002)
                {
                    matchDTO.info.participants[i].perks.statPerks.defenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png";
                    matchDTO.info.participants[i].perks.statPerks.defenseDesc = "방어력 +6";
                }
                else if (matchDTO.info.participants[i].perks.statPerks.defense == 5003)
                {
                    matchDTO.info.participants[i].perks.statPerks.defenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png";
                    matchDTO.info.participants[i].perks.statPerks.defenseDesc = "마법 저항력 +8";
                }
            }
        }
        private List<ParticipantDTO> InitParticipants(List<ParticipantDTO> participantDTOs)
        {
            for (int i = 0; i < participantDTOs.Count; i++)
            {
                participantDTOs[i].championPhoto = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + participantDTOs[i].championName + ".png";
                participantDTOs[i].totalCs = participantDTOs[i].neutralMinionsKilled + participantDTOs[i].totalMinionsKilled;
                if (participantDTOs[i].deaths == 0)
                    participantDTOs[i].KDA = string.Format("{0:0.00}", (participantDTOs[i].assists + participantDTOs[i].kills));
                else
                {
                    double kda = (double)(participantDTOs[i].kills + participantDTOs[i].assists) / participantDTOs[i].deaths;
                    participantDTOs[i].KDA = string.Format("{0:0.00}", kda);
                }
            }
            return participantDTOs;
        }
        #endregion


    }
}
