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

        public string GetMatchList(string puuid)
        {
            string path = "match/v5/matches/by-puuid/" + puuid;

            var response = GET(GetAsiaMatchUrl(path));
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return content;
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
        public ParticipantDTO GetUserData(MatchDTO matchDTO)
        {
            ParticipantDTO userData = new ParticipantDTO();
            for(int i = 0; i < matchDTO.info.participants.Count; i++)
            {
                if (Constants.UserName == matchDTO.info.participants[i].summonerName)
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

    }
}
