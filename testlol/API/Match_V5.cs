using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Match_V5;
using testlol.Utills;

namespace testlol.API
{
    public class Match_V5 : Api
    {
        public Match_V5()
        {

        }

        public IEnumerable<string> GetMatchList(string puuid)  //ieumerable = get밖에 없어서 수정 불가
        {
            string path = "match/v5/matches/by-puuid/" + puuid;

            var response = GET(GetAsiaMatchUrl(path));
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IEnumerable<string>>(content);
            }
            else
            {
                return null;
            }
        }

        public List<RuneDTO> GetRune()
        {
            string path = "https://ddragon.leagueoflegends.com/cdn/13.7.1/data/ko_KR/runesReforged.json";

            var response = GET(path);
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<RuneDTO>>(content);
            }
            else
            {
                return null;
            }
        }

        public MatchDTO GetMatchData(string matchId)
        {
            string path = "match/v5/matches/" + matchId;

            var response = GET(GetAsiaUrl(path));
            string content=response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MatchDTO>(content);
            }
            else
            {
                return null;
            }
        }
        public ParticipantDTO GetUserData(MatchDTO matchDTO, string summonerName)
        {
            ParticipantDTO userData = new ParticipantDTO();
            for(int i = 0; i < matchDTO.info.participants.Count; i++)
            {
                if (summonerName == matchDTO.info.participants[i].summonerName)
                {
                    userData = matchDTO.info.participants[i];
                }

            }
            return userData;
        }
        public string ReturnItemPhoto(int Item)
        {
            return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/item/" + Item + ".png";
        }
        public void GetTeam(MatchDTO matchdata, List<ParticipantDTO> redteam, List<ParticipantDTO> blueteam)
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
        public List<ParticipantDTO> InitParticipants(List<ParticipantDTO> participantDTOs)
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
        public void GetTier(List<ParticipantDTO> team)
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
    }
}
