    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Managers;
using testlol.Models.DTOs.Sumonner_V4;

namespace testlol.API
{
    public class Summoner_V4 :Api
    {
        public Summoner_V4()
        {

        }

        public SummonerDTO GetSummonerByName(string SummonerName)
        {
            string path = "summoner/v4/summoners/by-name/" + SummonerName;

            var response = GET(GetUrl(path));

            var result = ApiManager.Instance.ReturnSummoner(response);
            
            return result;
        }
    }
}
