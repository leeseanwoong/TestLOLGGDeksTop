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

        public IEnumerable<string> GetMatchList(string puuid) 
        {
            string path = "match/v5/matches/by-puuid/" + puuid;

            var response = GET(GetAsiaMatchUrl(path));

            var result = ApiManager.Instance.ReturnMatchList(response);

            return result;
        }

        public List<RuneDTO> GetRune()
        {
            string path = "https://ddragon.leagueoflegends.com/cdn/13.14.1/data/ko_KR/runesReforged.json";

            var response = GET(path);

            var result = ApiManager.Instance.ReturnRune(response);

            return result;
        }

        public MatchDTO GetMatchData(string matchId)
        {
            string path = "match/v5/matches/" + matchId;

            var response = GET(GetAsiaUrl(path));

            var result = ApiManager.Instance.ReturnMatchData(response);

            return result;
        }

    }
}
