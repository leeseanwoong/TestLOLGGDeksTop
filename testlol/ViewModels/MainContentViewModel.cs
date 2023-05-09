    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testlol.ViewModels
{
    internal class MainContentViewModel : ViewModelBase
    {
        private ViewModelBase _mainContent;

        public ViewModelBase MainContent
        {
            get => _mainContent;
            set
            {
                SetProperty(ref _mainContent, value);
                RaisePropertyChanged(nameof(MainContent));
            }
        }
    }
}
