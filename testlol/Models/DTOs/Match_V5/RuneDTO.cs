using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class RuneDTO
    {
        public int id { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public List<RunesDTO> slots { get; set; }
    }
}
