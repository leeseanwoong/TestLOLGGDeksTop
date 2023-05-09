using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using testlol.API;
using testlol.Models.DTOs;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.Scripts;
using testlol.Utills;

namespace testlol
{
    class ConnViewModel
    {
        public  RiotClientManager clientManager;

        private DelegateCommand buttonConnect;

        public ICommand ButtonConnect => buttonConnect = buttonConnect ?? new DelegateCommand(ButtonConnectCommand);


        private SummonerDTO GetSummoner(string SummerName)
        {
            Summoner_V4 summoner_V4 = new Summoner_V4();
            return summoner_V4.GetSummonerByName(SummerName);
        }

        

        private async void ButtonConnectCommand()
        {
            clientManager = new RiotClientManager();
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

                    Console.WriteLine($"Token : {ClientData.ToKen}");
                    Console.WriteLine($"Port : {ClientData.Port}");
                    Console.WriteLine($"ApiUri : {ClientData.ApiUrl}");
                }

                clientManager.Connect();

                MessageBox.Show("연결 성공");

                var msg = await clientManager.UsingApiEventJObject("Get", "/lol-summoner/v1/current-summoner");
                UserDTO username = JsonConvert.DeserializeObject<UserDTO>(msg.ToString());

                Constants.UserName = username.displayName;
                Constants.Summoner = GetSummoner(Constants.UserName);

            }
            catch
            {
                MessageBox.Show("연결 실패");
            }
        }
    }
}
