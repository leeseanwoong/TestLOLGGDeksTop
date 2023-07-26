using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Managers;
using testlol.Models.DTOs.League_V4;
using testlol.Models.DTOs.Sumonner_V4;

namespace testlol.API
{
    public class League_V4 : Api
    {
        public League_V4()
        {

        }

        private ApiManager apiManager = new ApiManager();

        public List<PositionDTO> GetPositions(string summonerId)
        {
            string path = "league/v4/entries/by-summoner/" + summonerId;

            var response = GET(GetUrl(path)); //여기 까지만
            
            var result = apiManager.ReturnPosition(response);

            return result;

            //여기서 부터 매니저에서 받아서 처리
            /*string content = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<PositionDTO>>(content);
            }
            else
            {
                return null;
            }*/
        }
        public PositionDTO GetPosition(SummonerDTO summoner)
        {
            League_V4 league = new League_V4();

            var position = league.GetPositions(summoner.Id).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();

            return position ?? new PositionDTO();
        }
    }
}
