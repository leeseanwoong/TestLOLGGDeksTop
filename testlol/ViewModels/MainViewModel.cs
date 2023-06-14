using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Managers;
using testlol.Types;

namespace testlol.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private MenuViewModel _menuViewModel;

        public MenuViewModel MenuViewModel
        {
            get
            {
                if (_menuViewModel == null)
                {
                    _menuViewModel = new MenuViewModel();
                    _menuViewModel.Items = new ObservableCollection<MenuItemViewModel>(FunctionMenuManager.Instance.GetFunctionListData().Select(MenuItemViewModel.From));
                    _menuViewModel.ChangedSelectedType += (_, __) => OnChangedViewModel();
                }
                return _menuViewModel;
            }
        }
        private MainContentViewModel _mainContentViewModel;

        public MainContentViewModel MainContentViewModel
        {
            get
            {
                if (_mainContentViewModel == null)
                {
                    _mainContentViewModel = new MainContentViewModel();
                }
                return _mainContentViewModel;
            }
        }

        private ViewModelBase _mainContent;

        public ViewModelBase MainContent
        {
            get => _mainContent;
            set => SetProperty(ref _mainContent, value);
        }

        private void OnChangedViewModel()
        {
            switch (_menuViewModel.SelectedType.Type)
            {
                case FunctionType.Home:
                    MainContentViewModel.MainContent = new HomeViewModel();
                    break;
                case FunctionType.Record:
                    MainContentViewModel.MainContent = new RecordViewModel();
                    break;
                case FunctionType.Queue:
                    MainContentViewModel.MainContent = new QueueViewModel();
                    break;
                case FunctionType.Search:
                    MainContentViewModel.MainContent = new SearchViewModel();
                    break;
            }
        }
    }
}
