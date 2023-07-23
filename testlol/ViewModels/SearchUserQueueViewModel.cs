using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.API;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Spectator_V4;
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class SearchUserQueueViewModel : ViewModelBase
    {
        public SearchUserQueueViewModel()
        {

        }
        public SearchUserQueueViewModel(CurrentGameInfoDTO gameInfo)
        {
            GameInfo = gameInfo;
            Match_V5 match = new Match_V5();
            GameMode = match.GetQueueType((int)GameInfo.gameQueueConfigId);
            List<BanChampionDTO> blue = new List<BanChampionDTO>();
            List<BanChampionDTO> red = new List<BanChampionDTO>();
            foreach (var item in gameInfo.bannedChampions)
            {
                if (item.teamId == 100)
                    blue.Add(item);
                else
                    red.Add(item);
            }
            Spectator_V4 spectator_V4 = new Spectator_V4();
            ChampionsDTO champions = spectator_V4.GetChampions();
            for (int i = 0; i < red.Count; i++)
            {
                foreach (var item in champions.data) 
                {
                    if (long.Parse(champions.data[item.Key].key) == red[i].championId)
                    {
                        red[i].championName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + champions.data[item.Key].id + ".png";
                    }

                }
            }
            for (int i = 0; i < blue.Count; i++)
            {
                foreach (var item in champions.data) 
                {
                    if (long.Parse(champions.data[item.Key].key) == blue[i].championId)
                    {
                        blue[i].championName = "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/champion/" + champions.data[item.Key].id + ".png";
                    }

                }
            }
            BlueBan = blue;
            RedBan = red;

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

        private List<BanChampionDTO> blueBan;
        public List<BanChampionDTO> BlueBan
        {
            get => blueBan;
            set => SetProperty(ref blueBan, value);
        }
        private List<BanChampionDTO> redBan;
        public List<BanChampionDTO> RedBan
        {
            get => redBan;
            set => SetProperty(ref redBan, value);
        }
        private ObservableCollection<QueueItemViewModel> innerRed
        { get; } = new ObservableCollection<QueueItemViewModel>();

        private ReadOnlyObservableCollection<QueueItemViewModel> redTeam;
        public ReadOnlyObservableCollection<QueueItemViewModel> RedTeam
        {
            get
            {

                if (redTeam == null && GameInfo != null )
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
                        GetPerks(item.perks, rune);
                        innerRed.Add(QueueItemViewModel.From(compare,position,league,item));
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

                if (blueTeam == null && GameInfo != null)
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

                        GetPerks(item.perks, rune);

                        innerBlue.Add(QueueItemViewModel.From(compare, position, league, item));
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
