using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using testlol.API;
using testlol.Managers;
using testlol.Models.DTOs;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.Scripts;
using testlol.Types;
using testlol.Utills;

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
                    if (Constants.Summoner == null)
                    {
                        MessageBox.Show("계정이 연결되있지 않습니다.");
                        return;
                    }
                    else
                        MainContentViewModel.MainContent = new HomeViewModel();
                    break;
                case FunctionType.Record:
                    if (Constants.Summoner == null)
                    {
                        MessageBox.Show("계정이 연결되있지 않습니다.");
                        return;
                    }
                    else
                        MainContentViewModel.MainContent = new RecordViewModel();
                    break;
                case FunctionType.Queue:
                    if (Constants.Summoner == null)
                    {
                        MessageBox.Show("계정이 연결되있지 않습니다.");
                        return;
                    }
                    else
                        MainContentViewModel.MainContent = new QueueViewModel();
                    break;
                case FunctionType.Search:
                    MainContentViewModel.MainContent = new SearchViewModel();
                    break;
                case FunctionType.Patch:
                    MainContentViewModel.MainContent = new PatchViewModel();
                    break;
            }
        }
    }
}
