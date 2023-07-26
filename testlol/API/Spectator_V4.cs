using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Managers;
using testlol.Models.DTOs.Spectator_V4;

namespace testlol.API
{
    public class Spectator_V4 : Api
    {

        public Spectator_V4()
        {

        }

        private ApiManager apiManager = new ApiManager();

        public CurrentGameInfoDTO GetCurrentGameInfo(string id)
        {
            string path = "spectator/v4/active-games/by-summoner/" + id;

            var response = GET(GetUrl(path));

            var result = apiManager.ReturnCurrentGameInfo(response);

            return result;
        }
        public ChampionsDTO GetChampions()
        {
            string path = "http://ddragon.leagueoflegends.com/cdn/13.14.1/data/en_US/champion.json";

            var response = GET(path);
            
            var result = apiManager.ReturnChampion(response);

            return result;
        }
    }
}
