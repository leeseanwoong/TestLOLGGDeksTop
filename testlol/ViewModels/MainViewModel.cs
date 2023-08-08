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
using System.Windows.Threading;
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
        private RiotClientManager riotClientManager;

        private Object _lock = new Object();

        private DispatcherTimer timer;
        public MainViewModel()
        {
            IsLoading = true;
            riotClientManager = new RiotClientManager();

            riotClientManager.LeagueClosed += RiotClientManager_LeagueClosed;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(7); // 7초마다 호출
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void RiotClientManager_LeagueClosed()
        {
            lock (_lock)
            {
                timer.Start();
                IsLoading = true;
            }
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                lock (_lock)
                {
                    riotClientManager.Connect();
                }
                
                var response = await riotClientManager.UsingApiEventJObject("Get", "lol-summoner/v1/current-summoner");
                UserDTO user = JsonConvert.DeserializeObject<UserDTO>(response.ToString());

                if (user.displayName != null)
                {
                    timer.Stop();

                    UserDataManager.Instance.Summoner = GetSummoner(user.displayName);

                    lock(_lock)
                    {
                        IsLoading=false;
                        MessageBox.Show("연결 완료");
                    }
                    
                }
            }
            catch
            {
                Console.WriteLine("Connection Failed");
            }


        }

        private SummonerDTO GetSummoner(string SummerName)
        {
            Summoner_V4 summoner_V4 = new Summoner_V4();
            return summoner_V4.GetSummonerByName(SummerName);
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set=>SetProperty(ref isLoading, value);
        }

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
                    if (UserDataManager.Instance.Summoner == null)
                    {
                        MessageBox.Show("계정이 연결되있지 않습니다.");
                        return;
                    }
                    else
                        MainContentViewModel.MainContent = new HomeViewModel();
                    break;
                case FunctionType.Record:
                    if (UserDataManager.Instance.Summoner == null)
                    {
                        MessageBox.Show("계정이 연결되있지 않습니다.");
                        return;
                    }
                    else
                        MainContentViewModel.MainContent = new RecordViewModel();
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
