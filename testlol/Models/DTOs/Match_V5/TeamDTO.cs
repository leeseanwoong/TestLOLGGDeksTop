using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class TeamDTO
    {
        public int teamId { get; set; }
        public bool win { get; set; }
        public ObjectivesDTO objectives { get; set; }
    }
}
