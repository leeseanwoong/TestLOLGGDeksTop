using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Match_V5;
using testlol.Models.DTOs.Spectator_V4;
using testlol.Models.DTOs.Sumonner_V4;

namespace testlol.Managers
{
    public class ApiManager
    {
        public ApiManager()
        {
                
        }
        public List<PositionDTO> ReturnPosition(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<PositionDTO>>(content);
            }
            else
            {
                return null;
            }
        }

        public SummonerDTO ReturnSummoner(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<SummonerDTO>(content);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> ReturnMatchList(HttpResponseMessage response)
        {
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
        public List<RuneDTO> ReturnRune(HttpResponseMessage response)
        {
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

        public MatchDTO ReturnMatchData(HttpResponseMessage response) 
        {
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MatchDTO>(content);
            }
            else
            {
                return null;
            }
        }

        public CurrentGameInfoDTO ReturnCurrentGameInfo(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<CurrentGameInfoDTO>(content);
            }
            else
            {
                return null;
            }
        }

        public ChampionsDTO ReturnChampion(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ChampionsDTO>(content);
            }
            else
            {
                return null;
            }
        }
    }
}
