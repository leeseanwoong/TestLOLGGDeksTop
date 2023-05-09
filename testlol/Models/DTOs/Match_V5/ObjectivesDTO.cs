using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class ObjectivesDTO
    {
        public ObjectiveDTO baron { get; set; }
        public ObjectiveDTO champion { get; set; }
        public ObjectiveDTO dragon { get; set; }
        public ObjectiveDTO inhibitor { get; set; } //억제기
        public ObjectiveDTO riftHerald { get; set; } //전령
        public ObjectiveDTO tower { get; set; }
    }
}
