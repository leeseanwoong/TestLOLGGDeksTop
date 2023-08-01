using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using testlol.Managers;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Spectator_V4;
using testlol.Utills;

namespace testlol.API
{
    public class Match_V5 : Api
    {
        public Match_V5()
        {

        }

        private ApiManager apiManager = new ApiManager();

        public IEnumerable<string> GetMatchList(string puuid)  //ieumerable = get밖에 없어서 수정 불가
        {
            string path = "match/v5/matches/by-puuid/" + puuid;

            var response = GET(GetAsiaMatchUrl(path));

            var result = apiManager.ReturnMatchList(response);

            return result;
        }

        public List<RuneDTO> GetRune()
        {
            string path = "https://ddragon.leagueoflegends.com/cdn/13.14.1/data/ko_KR/runesReforged.json";

            var response = GET(path);

            var result = apiManager.ReturnRune(response);

            return result;
        }

        public MatchDTO GetMatchData(string matchId)
        {
            string path = "match/v5/matches/" + matchId;

            var response = GET(GetAsiaUrl(path));

            var result = apiManager.ReturnMatchData(response);

            return result;
        }

        #region Utill
        public ParticipantDTO GetUserData(MatchDTO matchDTO, string summonerName)
        {
            ParticipantDTO userData = new ParticipantDTO();

            foreach (var item in matchDTO.info.participants)
            {
                if (summonerName == item.summonerName)
                {
                    userData = item;
                }
            }

            return userData;
        }
        public string ReturnItemPhoto(int Item)
        {
            return "http://ddragon.leagueoflegends.com/cdn/13.14.1/img/item/" + Item + ".png";
        }
        public void GetTeam(MatchDTO matchdata, List<ParticipantDTO> redteam, List<ParticipantDTO> blueteam)
        {
            int bluetotalkill = matchdata.info.participants.Where(p => p.teamId == 100).Sum(p => p.kills);
            int redtotalkill = matchdata.info.participants.Where(p => p.teamId == 200).Sum(p => p.kills);

            foreach (var item in matchdata.info.participants)
            {
                // 팀에 따라 적절한 킬 합계를 가져옴
                int teamTotalKill = item.teamId == 100 ? bluetotalkill : redtotalkill;

                // killRate 계산 및 설정
                item.killRate = teamTotalKill == 0 ? 0 : (double)(item.kills + item.assists) / teamTotalKill;
            }

            // 팀 구성원을 redteam과 blueteam 리스트로 나누기
            redteam.AddRange(matchdata.info.participants.Where(p => p.teamId == 200));
            blueteam.AddRange(matchdata.info.participants.Where(p => p.teamId == 100));

        }
        public void GetArenaTeams(MatchDTO matchdata, Dictionary<int, List<ParticipantDTO>> teams)
        {
            teams.Clear();

            foreach (var player in matchdata.info.participants)
            {
                if (!teams.TryGetValue(player.placement, out List<ParticipantDTO> team))
                {
                    team = new List<ParticipantDTO>();
                    teams.Add(player.placement, team);
                }

                team.Add(player);
            }

            foreach (var team in teams.Values)
            {
                int teamTotalKill = team.Sum(p => p.kills);
                foreach (var player in team)
                {
                    player.killRate = teamTotalKill == 0 ? 0 : (double)(player.kills + player.assists) / teamTotalKill;
                }
            }
        }

        public int GetArenaMaxDamge(Dictionary<int, List<ParticipantDTO>> arena)
        {
            int maxDamge = 0;
            foreach (var participants in arena.Values)
            {
                foreach (var value in participants)
                {
                    if (maxDamge <= value.totalDamageDealtToChampions)
                        maxDamge = value.totalDamageDealtToChampions;
                }
            }
            return maxDamge;
        }
        public int GetArenaMaxDamgeTaken(Dictionary<int, List<ParticipantDTO>> arena)
        {
            int maxDamge = 0;
            foreach (var participants in arena.Values)
            {
                foreach (var value in participants)
                {
                    if (maxDamge <= value.totalDamageTaken)
                        maxDamge = value.totalDamageTaken;
                }
            }
            return maxDamge;
        }
        

        public string GetGameTime(long GameDuration)
        {
            TimeSpan t = TimeSpan.FromSeconds(GameDuration);
            return $"{t.Minutes}분 {t.Seconds}초";
        }

        public string GetWinLose(bool win)
        {
            if (win == true)
                return "승리";
            else
                return "패배";
        }

        public void GetPerksImg(List<RuneDTO> runes, MatchDTO matchDTO)
        {
            foreach (var participant in matchDTO.info.participants)
            {
                foreach (var style in participant.perks.styles)
                {
                    var matchingRune = runes.FirstOrDefault(rune => rune.id == style.style);
                    if (matchingRune != null)
                    {
                        style.styleIcon = "https://ddragon.leagueoflegends.com/cdn/img/" + matchingRune.icon;
                        style.styleName = matchingRune.name;

                        foreach (var selection in style.selections)
                        {
                            foreach (var slot in matchingRune.slots)
                            {
                                var matchingRuneInSlot = slot.runes.FirstOrDefault(rune => rune.id == selection.perk);
                                if (matchingRuneInSlot != null)
                                {
                                    selection.perkImage = "https://ddragon.leagueoflegends.com/cdn/img/" + matchingRuneInSlot.icon;
                                    selection.perkName = matchingRuneInSlot.name;
                                }
                            }
                        }
                    }
                }
            }
        }  

        public void GetStatsImg(List<RuneDTO> runes, MatchDTO matchDTO)
        {
            var perkImageDict = new Dictionary<int, string>()
            {
                { 5008, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png" }, // 적응형 능력치
                { 5005, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsattackspeedicon.png" }, // 공격 속도 + 10%
                { 5007, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodscdrscalingicon.png" }, // 스킬 가속 + 8
                { 5002, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png" }, // 방어력 +6
                { 5003, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png" }, // 마법 저항력 +8
                { 5001, "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodshealthscalingicon.png" } // 체력 +15~140 (레벨에 비례)
            };

            foreach (var item in matchDTO.info.participants)
            {
                item.perks.statPerks.offenseImg = perkImageDict.TryGetValue(item.perks.statPerks.offense, out string offenseImg) ? offenseImg : "";
                item.perks.statPerks.offenseDesc = GetPerkDescription(item.perks.statPerks.offense);

                item.perks.statPerks.flexImg = perkImageDict.TryGetValue(item.perks.statPerks.flex, out string flexImg) ? flexImg : "";
                item.perks.statPerks.flexDesc = GetPerkDescription(item.perks.statPerks.flex);

                item.perks.statPerks.defenseImg = perkImageDict.TryGetValue(item.perks.statPerks.defense, out string defenseImg) ? defenseImg : "";
                item.perks.statPerks.defenseDesc = GetPerkDescription(item.perks.statPerks.defense);
            }
        }

        private string GetPerkDescription(int perkId)
        {
            switch (perkId)
            {
                case 5008:
                    return "적응형 능력치 + 9";
                case 5005:
                    return "공격 속도 + 10%";
                case 5007:
                    return "스킬 가속 + 8";
                case 5002:
                    return "방어력 +6";
                case 5003:
                    return "마법 저항력 +8";
                case 5001:
                    return "체력 +15~140 (레벨에 비례)";
                default:
                    return "";
            }
        }
        public List<ParticipantDTO> InitParticipants(List<ParticipantDTO> participantDTOs)
        {
            foreach (var item in participantDTOs)
            {
                item.championPhoto = "http://ddragon.leagueoflegends.com/cdn/13.14.1/img/champion/" + item.championName + ".png";
                item.totalCs = item.neutralMinionsKilled + item.totalMinionsKilled;
                if (item.deaths == 0)
                    item.KDA = string.Format("{0:0.00}", (item.assists + item.kills));
                else
                {
                    double kda = (double)(item.kills + item.assists) / item.deaths;
                    item.KDA = string.Format("{0:0.00}", kda);
                }
            }
            return participantDTOs;
        }

        public void GetPerks(PerkDTO perk, List<RuneDTO> runes)
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

    private string GetDefaultIconUrl(int perkId)
        {
            switch (perkId)
            {
                case 5008:
                    return "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png";
                case 5005:
                    return "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsattackspeedicon.png";
                case 5007:
                    return "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodscdrscalingicon.png";
                case 5002:
                    return "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png";
                case 5003:
                    return "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png";
                case 5001:
                    return "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodshealthscalingicon.png";
                default:
                    return null;
            }
        }
        public void GetTier(List<ParticipantDTO> team)
        {
            League_V4 league = new League_V4();

            foreach (var item in team)
            {
                var position = league.GetPositions(item.summonerId).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();
                if (position == null)
                    item.tier = "Unranked";
                else
                    item.tier = position.Tier + " " + position.Rank;
            }
        }

        #endregion
    }
}
