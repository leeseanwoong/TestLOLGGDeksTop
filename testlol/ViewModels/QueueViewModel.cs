using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using testlol.API;
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
            GameInfo = spectator_V4.GetCurrentGameInfo(Constants.Summoner.Id);
            if (GameInfo == null)
            {
                Visibility = "Hidden";
                GameMode = "현재 게임중이 아닙니다.";
            }
            else
            {
                Visibility = "Visible";
                Match_V5 match = new Match_V5();
                GameMode = match.GetQueueType((int)GameInfo.gameQueueConfigId);
            }
        }
        private string visibility;
        public string Visibility
        {
            get => visibility;
            set => SetProperty(ref visibility, value);
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

                if (redTeam == null && GameInfo != null && Constants.Summoner != null)
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
                    foreach (var item in participants)
                    {
                        if (item.teamid == 200)
                            red.Add(item);
                    }
                    for (int i = 0; i < red.Count; i++)
                    {
                        var position = league.GetPositions(red[i].summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
                        if (position == null)
                            position.Tier = "UnRanked";

                        foreach (var item in champions.data) // 챔피언 아이디 값으로 이름 찾기
                        {
                            if (long.Parse(champions.data[item.Key].key) == red[i].championid)
                            {
                                red[i].championName = champions.data[item.Key].id;
                            }

                        }

                        GetPerks(red[i].perks, rune);
                        innerRed.Add(new QueueItemViewModel()
                        {
                            tier = position.Tier + " " + position.Rank,
                            SummonerName = red[i].summonerName,
                            Summoner1Casts = league.GetSpellName((int)red[i].spell1Id),
                            Summoner2Casts = league.GetSpellName((int)red[i].spell2Id),
                            Win = position.Wins + "승 " + position.Losses + "패 " + "(" + string.Format("{0:P0}", (double)position.Wins / (position.Wins + position.Losses)) + ")",
                            PrimaryPerks = red[i].perks.perksImgs[0],
                            SubPerks = red[i].perks.perkSubStyleIcon,
                            ChampionName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + red[i].championName + ".png",
                        });
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

                if (blueTeam == null && GameInfo != null && Constants.Summoner != null)
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
                    foreach (var item in participants)
                    {
                        if (item.teamid == 100)
                            blue.Add(item);
                    }
                    for (int i = 0; i < blue.Count; i++)
                    {
                        var position = league.GetPositions(blue[i].summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
                        if (position == null)
                            position.Tier = "UnRanked";
                        foreach (var item in champions.data)
                        {
                            if (long.Parse(champions.data[item.Key].key) == blue[i].championid)
                            {
                                blue[i].championName = champions.data[item.Key].id;
                            }

                        }

                        GetPerks(blue[i].perks, rune);

                        innerRed.Add(new QueueItemViewModel()
                        {
                            tier = position.Tier + " " + position.Rank,
                            SummonerName = blue[i].summonerName,
                            Summoner1Casts = league.GetSpellName((int)blue[i].spell1Id),
                            Summoner2Casts = league.GetSpellName((int)blue[i].spell2Id),
                            Win = position.Wins + "승 " + position.Losses + "패 " + "(" + string.Format("{0:P0}", (double)position.Wins / (position.Wins + position.Losses)) + ")",
                            PrimaryPerks = blue[i].perks.perksImgs[0],
                            SubPerks = blue[i].perks.perkSubStyleIcon,
                            ChampionName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + blue[i].championName + ".png",
                        });
                    }

                }
                return blueTeam;
            }
        }

        private void GetPerks(PerkDTO perk, List<RuneDTO> runes)
        {
            perk.perksImgs = new List<string>();
            for (int i = 0; i < runes.Count; i++) //스타일 아이콘 찾기
            {
                if (perk.perkStyle == runes[i].id)
                {
                    perk.perksStyleIcon = "https://ddragon.leagueoflegends.com/cdn/img/" + runes[i].icon;
                    break;
                }

            }
            for (int i = 0; i < runes.Count; i++)
            {
                if (perk.perkSubStyle == runes[i].id)
                {
                    perk.perkSubStyleIcon = "https://ddragon.leagueoflegends.com/cdn/img/" + runes[i].icon;
                    break;
                }
            }
            for (int i = 0; i < perk.perkIds.Count; i++)
            {
                for (int j = 0; j < runes.Count; j++)
                {
                    for (int k = 0; k < runes[j].slots.Count; k++)
                    {
                        for (int t = 0; t < runes[j].slots[k].runes.Count; t++)
                        {
                            if (perk.perkIds[i] == runes[j].slots[k].runes[t].id)
                            {

                                perk.perksImgs.Add("https://ddragon.leagueoflegends.com/cdn/img/" + runes[j].slots[k].runes[t].icon);
                            }
                            else
                            {
                                if (perk.perkIds[i] == 5008) // 맨 위쪽
                                {
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png"); // 적응형 능력치
                                }
                                else if (perk.perkIds[i] == 5005)
                                {
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsattackspeedicon.png");

                                }
                                else if (perk.perkIds[i] == 5007)
                                {
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodscdrscalingicon.png");

                                }
                                else if (perk.perkIds[i] == 5002)
                                {
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png");

                                }
                                else if (perk.perkIds[i] == 5003)
                                {
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png");

                                }

                                else if (perk.perkIds[i] == 5001)
                                {
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodshealthscalingicon.png");
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
