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

        public string GetKey(string path)
        {
            StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }
    }
}
