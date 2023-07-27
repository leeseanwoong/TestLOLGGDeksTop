using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Sumonner_V4;

namespace testlol.Managers
{
    public class UserDataManager
    {
        private static UserDataManager _instance;
        public static UserDataManager Instance => _instance ?? (_instance = new UserDataManager());

        public SummonerDTO Summoner { get; set; }


    }
}
