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
            int bluetotalkill = 0;
            int redtotalkill = 0;

            foreach (var team in matchdata.info.participants)
            {
                if (team.teamId == 100)
                {
                    blueteam.Add(team);
                    bluetotalkill = bluetotalkill + team.kills;

                }
                else if (team.teamId == 200)
                {
                    redteam.Add(team);
                    redtotalkill = redtotalkill + team.kills;
                }
            }

            foreach (var item in matchdata.info.participants)
            {
                if (item.teamId == 100)
                {
                    foreach (var team in blueteam)
                    {
                        if (item.summonerName == team.summonerName)
                        {
                            item.killRate = (double)(item.kills + item.assists) / bluetotalkill;
                        }
                    }
                }
                else if (item.teamId == 200)
                {
                    foreach (var team in redteam)
                    {
                        if (item.summonerName == team.summonerName)
                        {
                            item.killRate = (double)(item.kills + item.assists) / redtotalkill;
                        }
                    }
                }
            }
        }

        public void GetPerks(PerkDTO perk, List<RuneDTO> runes)
        {
            perk.perksImgs = new List<string>();

            foreach (var rune in runes)
            {
                if (perk.perkStyle == rune.id)
                {
                    perk.perksStyleIcon = "https://ddragon.leagueoflegends.com/cdn/img/" + rune.icon;
                    break;
                }
            }
            foreach (var perks in perk.perkIds)
            {
                foreach (var rune in runes)
                {
                    foreach (var slot in rune.slots)
                    {
                        foreach (var item in slot.runes)
                        {
                            if (perks == item.id)
                                perk.perksImgs.Add("https://ddragon.leagueoflegends.com/cdn/img/" + item.icon);
                            else
                            {
                                if (perks == 5008) // 맨 위쪽
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png"); // 적응형 능력치
                                else if (perks == 5005)
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsattackspeedicon.png");
                                else if (perks == 5007)
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodscdrscalingicon.png");
                                else if (perks == 5002)
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png");
                                else if (perks == 5003)
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png");
                                else if (perks == 5001)
                                    perk.perksImgs.Add("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodshealthscalingicon.png");
                            }
                        }
                    }
                }
            }
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
        public void GetPerksImg(List<RuneDTO> rune, MatchDTO matchDTO)
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

        public void GetStatsImg(List<RuneDTO> rune, MatchDTO matchDTO)
        {
            foreach (var item in matchDTO.info.participants)
            {
                if (item.perks.statPerks.offense == 5008) // 맨 위쪽
                {
                    item.perks.statPerks.offenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png"; // 적응형 능력치
                    item.perks.statPerks.offenseDesc = "적응형 능력치 + 9";
                }
                else if (item.perks.statPerks.offense == 5005)
                {
                    item.perks.statPerks.offenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsattackspeedicon.png";
                    item.perks.statPerks.offenseDesc = "공격 속도 + 10%";
                }
                else if (item.perks.statPerks.offense == 5007)
                {
                    item.perks.statPerks.offenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodscdrscalingicon.png";
                    item.perks.statPerks.offenseDesc = "스킬 가속 + 8";
                }

                if (item.perks.statPerks.flex == 5008)
                {
                    item.perks.statPerks.flexImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsadaptiveforceicon.png";
                    item.perks.statPerks.flexDesc = "적응형 능력치 + 9";
                }
                else if (item.perks.statPerks.flex == 5002)
                {
                    item.perks.statPerks.flexImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png";
                    item.perks.statPerks.flexDesc = "방어력 +6";
                }
                else if (item.perks.statPerks.flex == 5003)
                {
                    item.perks.statPerks.flexImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png";
                    item.perks.statPerks.flexDesc = "마법 저항력 +8";
                }

                if (item.perks.statPerks.defense == 5001)
                {
                    item.perks.statPerks.defenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodshealthscalingicon.png";
                    item.perks.statPerks.defenseDesc = "체력 +15~140 (레벨에 비례)";
                }
                else if (item.perks.statPerks.defense == 5002)
                {
                    item.perks.statPerks.defenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsarmoricon.png";
                    item.perks.statPerks.defenseDesc = "방어력 +6";
                }
                else if (item.perks.statPerks.defense == 5003)
                {
                    item.perks.statPerks.defenseImg = "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/perk-images/statmods/statmodsmagicresicon.magicresist_fix.png";
                    item.perks.statPerks.defenseDesc = "마법 저항력 +8";
                }
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
