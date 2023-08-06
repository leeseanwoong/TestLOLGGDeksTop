using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using testlol.API;
using testlol.Managers;
using testlol.Models.DTOs;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class RecordListItemViewModel : ViewModelBase
    {
        public string QueueType { get; set; }
        public int Assists { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public string Item0 { get; set; }
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public string Item4 { get; set; }
        public string Item5 { get; set; }
        public string Item6 { get; set; }
        public string Win { get; set; }
        public string KDA { get; set; }
        public string Summoner1Casts { get; set; }
        public string Summoner2Casts { get; set; }
        public string ChampionPhoto { get; set; }
        public string GameDuration { get; set; }
        public List<ParticipantDTO> BlueTeam { get; set; }
        public List<ParticipantDTO> RedTeam { get; set; }
        public int TotalCS { get; set; }
        public string AvgCS { get; set; }
        public int ChampionLevel { get; set; }
        public string KillRate { get; set; }
        public string TotalGold { get; set; }
        public int TotalDetecedWard { get; set; }
        public string PrimaryPerks { get; set; }
        public string SubPerks { get; set; }
        public long gametime { get; set; }
        public List<TeamDTO> teams { get; set; }
        public PerksDTO Perks { get; set; }
        public Dictionary<int, List<ParticipantDTO>> ArenaTeams { get; set; }
        public Visibility BtnVisible { get; set; }

        public static RecordListItemViewModel From(string matchID)
        {
            Match_V5 match_V5 = new Match_V5();

            MatchDTO matchData = match_V5.GetMatchData(matchID);
            matchData.info.participants = LoLUtility.InitParticipants(matchData.info.participants);
            ParticipantDTO userData = LoLUtility.GetUserData(matchData, UserDataManager.Instance.Summoner.Name);

            List<RuneDTO> rune = match_V5.GetRune();

            List<ParticipantDTO> redTeam = new List<ParticipantDTO>();
            List<ParticipantDTO> blueTeam = new List<ParticipantDTO>();
            LoLUtility.GetTeam(matchData, redTeam, blueTeam);

            if (matchData.info.queueId == 1700)
            {
                Dictionary<int, List<ParticipantDTO>> arena = new Dictionary<int, List<ParticipantDTO>>();
                LoLUtility.GetArenaTeams(matchData, arena);

                return new RecordListItemViewModel()
                {
                    Assists = userData.assists,
                    Kills = userData.kills,
                    Deaths = userData.deaths,
                    KDA = userData.KDA,
                    Win = LoLUtility.GetWinLose(userData.win),
                    QueueType = LoLUtility.GetQueueType(matchData.info.queueId),
                    GameDuration = LoLUtility.GetGameTime(matchData.info.gameDuration),
                    ChampionPhoto = userData.championPhoto,
                    Summoner1Casts = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment1 + ".png",
                    Summoner2Casts = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment3 + ".png",
                    PrimaryPerks = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment2 + ".png",
                    SubPerks = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment4 + ".png",
                    Item0 = LoLUtility.ReturnItemPhoto(userData.item0),
                    Item1 = LoLUtility.ReturnItemPhoto(userData.item1),
                    Item3 = LoLUtility.ReturnItemPhoto(userData.item2),
                    Item2 = LoLUtility.ReturnItemPhoto(userData.item3),
                    Item4 = LoLUtility.ReturnItemPhoto(userData.item4),
                    Item5 = LoLUtility.ReturnItemPhoto(userData.item5),
                    Item6 = LoLUtility.ReturnItemPhoto(userData.item6),
                    ChampionLevel = userData.champLevel,
                    TotalGold = string.Format("{0:#,###0}", userData.goldEarned),
                    KillRate = string.Format("{0:P0}", userData.killRate),
                    gametime = matchData.info.gameDuration,
                    ArenaTeams = arena,
                    Perks = userData.perks,
                    BlueTeam = blueTeam,
                    RedTeam = redTeam,
                    BtnVisible = Visibility.Hidden,
                };
            }
            else
            {
                LoLUtility.GetPerksImg(rune, matchData);
                LoLUtility.GetStatsImg(rune, matchData);

                return new RecordListItemViewModel()
                {
                    Assists = userData.assists,
                    Kills = userData.kills,
                    Deaths = userData.deaths,
                    KDA = userData.KDA,
                    Win = LoLUtility.GetWinLose(userData.win),
                    QueueType = LoLUtility.GetQueueType(matchData.info.queueId),
                    GameDuration = LoLUtility.GetGameTime(matchData.info.gameDuration),
                    ChampionPhoto = userData.championPhoto,
                    Summoner1Casts = LoLUtility.GetSpellName(userData.Summoner1Id),
                    Summoner2Casts = LoLUtility.GetSpellName(userData.Summoner2Id),
                    PrimaryPerks = userData.perks.styles[0].selections[0].perkImage,
                    SubPerks = userData.perks.styles[1].styleIcon,
                    Item0 = LoLUtility.ReturnItemPhoto(userData.item0),
                    Item1 = LoLUtility.ReturnItemPhoto(userData.item1),
                    Item3 = LoLUtility.ReturnItemPhoto(userData.item2),
                    Item2 = LoLUtility.ReturnItemPhoto(userData.item3),
                    Item4 = LoLUtility.ReturnItemPhoto(userData.item4),
                    Item5 = LoLUtility.ReturnItemPhoto(userData.item5),
                    Item6 = LoLUtility.ReturnItemPhoto(userData.item6),
                    BlueTeam = blueTeam,
                    RedTeam = redTeam,
                    ChampionLevel = userData.champLevel,
                    TotalCS = userData.totalCs,
                    AvgCS = string.Format("{0:0.0}", userData.totalCs / ((double)matchData.info.gameDuration / 60)),
                    TotalDetecedWard = userData.detectorWardsPlaced,
                    TotalGold = string.Format("{0:#,###0}", userData.goldEarned),
                    KillRate = string.Format("{0:P0}", userData.killRate),
                    gametime = matchData.info.gameDuration,
                    teams = matchData.info.teams,
                    Perks = userData.perks,
                    BtnVisible = Visibility.Visible,
                };
            }
        }

        public static RecordListItemViewModel From(string matchID, SummonerDTO summoner)
        {
            Match_V5 match_V5 = new Match_V5();

            MatchDTO matchData = match_V5.GetMatchData(matchID);
            matchData.info.participants = LoLUtility.InitParticipants(matchData.info.participants);
            ParticipantDTO userData = LoLUtility.GetUserData(matchData, summoner.Name);

            List<RuneDTO> rune = match_V5.GetRune();

            List<ParticipantDTO> redTeam = new List<ParticipantDTO>();
            List<ParticipantDTO> blueTeam = new List<ParticipantDTO>();
            LoLUtility.GetTeam(matchData, redTeam, blueTeam);

            if (matchData.info.queueId == 1700)
            {
                Dictionary<int, List<ParticipantDTO>> arena = new Dictionary<int, List<ParticipantDTO>>();
                LoLUtility.GetArenaTeams(matchData, arena);

                return new RecordListItemViewModel()
                {
                    Assists = userData.assists,
                    Kills = userData.kills,
                    Deaths = userData.deaths,
                    KDA = userData.KDA,
                    Win = LoLUtility.GetWinLose(userData.win),
                    QueueType = LoLUtility.GetQueueType(matchData.info.queueId),
                    GameDuration = LoLUtility.GetGameTime(matchData.info.gameDuration),
                    ChampionPhoto = userData.championPhoto,
                    Summoner1Casts = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment1 + ".png",
                    Summoner2Casts = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment3 + ".png",
                    PrimaryPerks = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment2 + ".png",
                    SubPerks = "https://z.fow.kr/img/arena/augment/" + userData.playerAugment4 + ".png",
                    Item0 = LoLUtility.ReturnItemPhoto(userData.item0),
                    Item1 = LoLUtility.ReturnItemPhoto(userData.item1),
                    Item3 = LoLUtility.ReturnItemPhoto(userData.item2),
                    Item2 = LoLUtility.ReturnItemPhoto(userData.item3),
                    Item4 = LoLUtility.ReturnItemPhoto(userData.item4),
                    Item5 = LoLUtility.ReturnItemPhoto(userData.item5),
                    Item6 = LoLUtility.ReturnItemPhoto(userData.item6),
                    ChampionLevel = userData.champLevel,
                    TotalGold = string.Format("{0:#,###0}", userData.goldEarned),
                    KillRate = string.Format("{0:P0}", userData.killRate),
                    gametime = matchData.info.gameDuration,
                    ArenaTeams = arena,
                    Perks = userData.perks,
                    BlueTeam = blueTeam,
                    RedTeam = redTeam,
                    BtnVisible = Visibility.Hidden,
                };
            }
            else
            {
                LoLUtility.GetPerksImg(rune, matchData);
                LoLUtility.GetStatsImg(rune, matchData);

                return new RecordListItemViewModel()
                {
                    Assists = userData.assists,
                    Kills = userData.kills,
                    Deaths = userData.deaths,
                    KDA = userData.KDA,
                    Win = LoLUtility.GetWinLose(userData.win),
                    QueueType = LoLUtility.GetQueueType(matchData.info.queueId),
                    GameDuration = LoLUtility.GetGameTime(matchData.info.gameDuration),
                    ChampionPhoto = userData.championPhoto,
                    Summoner1Casts = LoLUtility.GetSpellName(userData.Summoner1Id),
                    Summoner2Casts = LoLUtility.GetSpellName(userData.Summoner2Id),
                    PrimaryPerks = userData.perks.styles[0].selections[0].perkImage,
                    SubPerks = userData.perks.styles[1].styleIcon,
                    Item0 = LoLUtility.ReturnItemPhoto(userData.item0),
                    Item1 = LoLUtility.ReturnItemPhoto(userData.item1),
                    Item3 = LoLUtility.ReturnItemPhoto(userData.item2),
                    Item2 = LoLUtility.ReturnItemPhoto(userData.item3),
                    Item4 = LoLUtility.ReturnItemPhoto(userData.item4),
                    Item5 = LoLUtility.ReturnItemPhoto(userData.item5),
                    Item6 = LoLUtility.ReturnItemPhoto(userData.item6),
                    BlueTeam = blueTeam,
                    RedTeam = redTeam,
                    ChampionLevel = userData.champLevel,
                    TotalCS = userData.totalCs,
                    AvgCS = string.Format("{0:0.0}", userData.totalCs / ((double)matchData.info.gameDuration / 60)),
                    TotalDetecedWard = userData.detectorWardsPlaced,
                    TotalGold = string.Format("{0:#,###0}", userData.goldEarned),
                    KillRate = string.Format("{0:P0}", userData.killRate),
                    gametime = matchData.info.gameDuration,
                    teams = matchData.info.teams,
                    Perks = userData.perks,
                    BtnVisible = Visibility.Visible,
                };
            }
        }

    }
}
