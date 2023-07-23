using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Sumonner_V4;

namespace testlol.Utills
{
    //  이 값을 매니저에서 들고 있음 , 매니저를 싱글톤으로 변경 or 매니저를 하나 더 만들어서 값 저장해놓기
    public static class Constants
    {
        public static SummonerDTO Summoner { get; set; }
        public static string UserName { get; set; }
    }
}
