using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Match_V5;

namespace testlol.API
{
    public class Api
    {
        private string Key { get; set; }

        public Api()
        {
            Key = GetKey("API/Key.txt");
        }

        protected HttpResponseMessage GET(string URL)
        {
            using (HttpClient client = new HttpClient())
            {
                var result = client.GetAsync(URL);
                result.Wait();

                return result.Result;
            }
        }

        protected string GetUrl(string path)
        {
            return "https://kr.api.riotgames.com/lol/" + path + "?api_key=" + Key;
        }
        protected string GetAsiaUrl(string path)
        {
            return "https://asia.api.riotgames.com/lol/" + path + "?api_key=" + Key;
        }
        protected string GetAsiaMatchUrl(string path)
        {
            return "https://asia.api.riotgames.com/lol/" + path + "/ids?start=0&count=10&api_key=" + Key;
        }

        public string GetQueueType(int QueueId)
        {
            if (QueueId == 420)
                return "솔랭";
            else if (QueueId == 430)
                return "일반";
            else if (QueueId == 440)
                return "자유랭크";
            else if (QueueId == 450)
                return "칼바람";
            else if (QueueId == 1700)
                return "아레나";
            else
                return "";
        }

        public string GetSpellName(int spell)
        {
            if (spell == 1)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerBoost.png";
            }
            else if (spell == 3)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerExhaust.png";
            }
            else if (spell == 4)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerFlash.png";
            }
            else if (spell == 6)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerHaste.png";
            }
            else if (spell == 7)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerHeal.png";
            }
            else if (spell == 11)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerSmite.png";
            }
            else if (spell == 12)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerTeleport.png";
            }
            else if (spell == 13)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerMana.png";
            }
            else if (spell == 14)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerDot.png";
            }
            else if (spell == 21)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerBarrier.png";
            }
            else if (spell == 30)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerPoroRecall.png";
            }
            else if (spell == 31)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerPoroThrow.png";
            }
            else if (spell == 32)
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/SummonerSnowball.png";
            }
            else
            {
                return "http://ddragon.leagueoflegends.com/cdn/13.6.1/img/spell/Summoner_UltBookPlaceholder.png";
            }

        }



        public string GetKey(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }
    }
}
