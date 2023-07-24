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
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private bool isConnected = false;

        public RiotClientManager clientManager = new RiotClientManager();

        private DispatcherTimer timer = new DispatcherTimer();
        

        private SummonerDTO GetSummoner(string SummerName)
        {
            Summoner_V4 summoner_V4 = new Summoner_V4();
            return summoner_V4.GetSummonerByName(SummerName);
        }
        public MenuViewModel()
        {
            ClientConnection();
        }

        private void ClientConnection()
        {
            if (!isConnected)
            {
                timer.Interval = TimeSpan.FromTicks(10000000);
                timer.Tick += async (s, a) =>
                {
                    try
                    {
                        Process[] processes = Process.GetProcessesByName(ClientData.CLIENT_NAME);
                        ClientData.clientProcess = processes[0];

                        ClientData.LeaguePath = Path.GetDirectoryName(ClientData.clientProcess.MainModule.FileName);
                        var lockFilePath = Path.Combine(ClientData.LeaguePath, "lockfile");

                        using (var fileStream = new FileStream(lockFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var reader = new StreamReader(fileStream))
                        {
                            var text = reader.ReadToEnd();
                            string[] items = text.Split(':');
                            ClientData.ToKen = items[3];
                            ClientData.Port = ushort.Parse(items[2]);
                            ClientData.ApiUrl = "https://127.0.0.1:" + ClientData.Port.ToString() + "/";

                        }

                        clientManager.Connect();

                        var msg = await clientManager.UsingApiEventJObject("Get", "/lol-summoner/v1/current-summoner");
                        UserDTO username = JsonConvert.DeserializeObject<UserDTO>(msg.ToString());
                        if (UserDataManager.Instance.Summoner == null)
                        {
                            UserDataManager.Instance.UserName = username.displayName;
                            UserDataManager.Instance.Summoner = GetSummoner(UserDataManager.Instance.UserName);
                            MessageBox.Show("연결 완료");
                            timer.Stop();
                        }

                    }
                    catch
                    {
                        Console.WriteLine("Connection Failed");
                    }
                };
                
                clientManager.LeagueClosed += OnLeagueColsed;
                timer.Start();

                isConnected = true;
            }
        }

        private void OnLeagueColsed()
        {
            MessageBox.Show("연결 해제");
            UserDataManager.Instance.Summoner = null;
            timer.Start();
        }

        private ObservableCollection<MenuItemViewModel> _items;

        public ObservableCollection<MenuItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private MenuItemViewModel _selectedType;

        public MenuItemViewModel SelectedType
        {
            get => _selectedType;
            set
            {
                if (SetProperty(ref _selectedType, value))
                {
                    OnChangedSelectedType();
                }
            }
        }

        public EventHandler ChangedSelectedType;

        private void OnChangedSelectedType()
        {
            ChangedSelectedType?.Invoke(this, EventArgs.Empty);
        }
    }
}
