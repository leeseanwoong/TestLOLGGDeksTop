using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.DTOs.Match_V5;

namespace testlol.ViewModels
{
    internal class PerksPopUpViewModel : ViewModelBase
    {
        public PerksPopUpViewModel(PerksDTO perksDTO)
        {
            Perks = perksDTO;
        }
        public PerksPopUpViewModel()
        {

        }

        private PerksDTO perks;
        public PerksDTO Perks
        {
            get => perks;
            set => SetProperty(ref perks, value);
        }
    }
}
