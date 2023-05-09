using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Spectator_V4;

namespace testlol.API
{
    public class Spectator_V4 : Api
    {

        public Spectator_V4()
        {

        }

        public CurrentGameInfoDTO GetCurrentGameInfo(string id)
        {
            string path = "spectator/v4/active-games/by-summoner/" + id;

            var response = GET(GetUrl(path));
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
    }
}
