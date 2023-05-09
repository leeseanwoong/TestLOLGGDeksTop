using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.Models.DTOs.Match_V5
{
    public class PerkStyleDTO
    {
        public string description { get; set; }
        public List<PerksStyleSelectionDTO> selections { get; set; }
        public int style { get; set; }
        public string styleName { get; set; }
        public string styleIcon { get; set; }
    }
}
